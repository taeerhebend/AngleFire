namespace AngleFire.Server.Factory
{
    public interface IPromptFactory
    {
        Task<T> GetCategoryPrompts<T>(string prompt);
        Task<T> GetPrompt<T>(string prompt);

        Task<T> GetCloudFunctionPrompts<T>(string prompt);

        Task<T> GetCloudFunctionCategories<T>();


        Task<T> GetIDEPrompts<T>(string prompt);

        Task<T> GetRegexPrompts<T>(string prompt);

    }
}
