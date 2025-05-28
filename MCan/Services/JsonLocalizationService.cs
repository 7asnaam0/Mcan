using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
namespace MCan.Services;

public class JsonLocalizationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _localizations = new();
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<JsonLocalizationService> _logger;

    public JsonLocalizationService(IWebHostEnvironment env, ILogger<JsonLocalizationService> logger)
    {
        _env = env;
        _logger = logger;

        // Log the resources path on startup
        var resourcesPath = Path.Combine(_env.ContentRootPath, "Resources");
        _logger.LogInformation($"Localization resources path: {resourcesPath}");
    }

    public string Get(string key, string culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var filePath = Path.Combine(_env.ContentRootPath, "Resources", $"{culture}.json");

        _logger.LogInformation($"Looking for: {filePath}");

        if (!File.Exists(filePath))
        {
            _logger.LogError($"File not found: {filePath}");
            return key;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            using JsonDocument doc = JsonDocument.Parse(json);

            var keyParts = key.Split('.');
            JsonElement current = doc.RootElement;

            foreach (var part in keyParts)
            {
                if (current.ValueKind != JsonValueKind.Object ||
                    !current.TryGetProperty(part, out current))
                {
                    _logger.LogWarning($"Key part '{part}' not found in path '{key}'");
                    return key;
                }
            }

            return current.GetString() ?? key;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error parsing {filePath}");
            return key;
        }
    }
    private List<string> GetAllKeys(Dictionary<string, object> dict, string prefix = "")
    {
        var keys = new List<string>();
        foreach (var kvp in dict)
        {
            var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
            if (kvp.Value is JsonElement el && el.ValueKind == JsonValueKind.Object)
            {
                var nested = JsonSerializer.Deserialize<Dictionary<string, object>>(el.GetRawText());
                keys.AddRange(GetAllKeys(nested, key));
            }
            else
            {
                keys.Add(key);
            }
        }
        return keys;
    }



    private Dictionary<string, string> Flatten(Dictionary<string, object> dict, string parentKey = "")
    {
        var result = new Dictionary<string, string>();

        foreach (var kvp in dict)
        {
            var newKey = string.IsNullOrEmpty(parentKey) ? kvp.Key : $"{parentKey}.{kvp.Key}";

            if (kvp.Value is JsonElement el)
            {
                switch (el.ValueKind)
                {
                    case JsonValueKind.Object:
                        var nestedDict = JsonSerializer.Deserialize<Dictionary<string, object>>(el.GetRawText());
                        foreach (var item in Flatten(nestedDict, newKey))
                        {
                            result[item.Key] = item.Value;
                        }
                        break;

                    case JsonValueKind.String:
                        result[newKey] = el.GetString();
                        break;

                    default:
                        result[newKey] = el.ToString();
                        break;
                }
            }
            else if (kvp.Value is string str)
            {
                result[newKey] = str;
            }
        }

        return result;
    }

}