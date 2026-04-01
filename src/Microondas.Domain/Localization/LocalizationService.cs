namespace Microondas.Domain.Localization;

public interface ILocalizationService
{
    string GetString(string key);
    void SetLanguage(string languageCode);
    string CurrentLanguage { get; }
    List<string> AvailableLanguages { get; }
}

public class LocalizationService : ILocalizationService
{
    private string _currentLanguage = "en";
    private Dictionary<string, Dictionary<string, string>> _resources = new();

    public string CurrentLanguage => _currentLanguage;
    public List<string> AvailableLanguages => new() { "en", "pt-BR" };

    public LocalizationService()
    {
        LoadResources();
    }

    public string GetString(string key)
    {
        if (_resources.ContainsKey(_currentLanguage) && 
            _resources[_currentLanguage].ContainsKey(key))
        {
            return _resources[_currentLanguage][key];
        }
        return key;
    }

    public void SetLanguage(string languageCode)
    {
        if (AvailableLanguages.Contains(languageCode))
        {
            _currentLanguage = languageCode;
        }
    }

    private void LoadResources()
    {
        _resources["en"] = new()
        {
            { "microwave.title", "Microwave Panel" },
            { "microwave.programs", "Programs" },
            { "microwave.manage_programs", "Manage Programs" },
            { "microwave.predefined", "Predefined" },
            { "microwave.custom", "Custom" },
            { "microwave.food", "Food" },
            { "microwave.time", "Time" },
            { "microwave.power", "Power" },
            { "microwave.instructions", "Instructions" },
            { "microwave.start", "START" },
            { "microwave.pause", "PAUSE/CANCEL" },
            { "microwave.quick_start", "QUICK START" },
            { "microwave.display_placeholder", "00:00" },
            { "microwave.power_placeholder", "Power (1-10)" },
            { "microwave.status", "Status" },
            { "microwave.remaining_time", "Remaining Time" },
            { "microwave.total_time", "Total Time" },
            { "microwave.seconds", "s" },
            { "program.create_new", "Create New Custom Program" },
            { "program.name", "Program Name" },
            { "program.name_placeholder", "Enter program name" },
            { "program.food_placeholder", "Enter food type" },
            { "program.time_seconds", "Time (seconds)" },
            { "program.heating_char", "Heating Character" },
            { "program.heating_char_placeholder", "Enter a single character" },
            { "program.add_program", "Add Program" },
            { "program.delete", "Delete" },
            { "program.error", "Error" },
            { "program.success", "Success" },
            { "program.custom_programs", "Custom Programs" },
            { "program.no_custom", "No custom programs yet" },
            { "language.select", "Language" },
            { "language.english", "English" },
            { "language.portuguese_br", "Português (Brasil)" },
        };

        _resources["pt-BR"] = new()
        {
            { "microwave.title", "Painel do Microondas" },
            { "microwave.programs", "Programas" },
            { "microwave.manage_programs", "Gerenciar Programas" },
            { "microwave.predefined", "Predefinido" },
            { "microwave.custom", "Personalizado" },
            { "microwave.food", "Alimento" },
            { "microwave.time", "Tempo" },
            { "microwave.power", "Potência" },
            { "microwave.instructions", "Instruções" },
            { "microwave.start", "INICIAR" },
            { "microwave.pause", "PAUSAR/CANCELAR" },
            { "microwave.quick_start", "INÍCIO RÁPIDO" },
            { "microwave.display_placeholder", "00:00" },
            { "microwave.power_placeholder", "Potência (1-10)" },
            { "microwave.status", "Status" },
            { "microwave.remaining_time", "Tempo Restante" },
            { "microwave.total_time", "Tempo Total" },
            { "microwave.seconds", "s" },
            { "program.create_new", "Criar Novo Programa Personalizado" },
            { "program.name", "Nome do Programa" },
            { "program.name_placeholder", "Digite o nome do programa" },
            { "program.food_placeholder", "Digite o tipo de alimento" },
            { "program.time_seconds", "Tempo (segundos)" },
            { "program.heating_char", "Caractere de Aquecimento" },
            { "program.heating_char_placeholder", "Digite um único caractere" },
            { "program.add_program", "Adicionar Programa" },
            { "program.delete", "Deletar" },
            { "program.error", "Erro" },
            { "program.success", "Sucesso" },
            { "program.custom_programs", "Programas Personalizados" },
            { "program.no_custom", "Nenhum programa personalizado ainda" },
            { "language.select", "Idioma" },
            { "language.english", "English" },
            { "language.portuguese_br", "Português (Brasil)" },
        };
    }
}

