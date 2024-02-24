using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using RepoUtils;


namespace KernelSyntaxExamples;
public class CredentialHelper
{
    public static DefaultAzureCredential GetCredential()
    {
        var authOptions = new DefaultAzureCredentialOptions
        {
            ExcludeEnvironmentCredential = false,
            ExcludeManagedIdentityCredential = false,
            ExcludeSharedTokenCacheCredential = false,
            ExcludeAzureCliCredential = false,//ExcludeAzureCliCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeInteractiveBrowserCredential = true,
        };



        return new DefaultAzureCredential(authOptions);
    }



    public static AzureChatCompletion GetAzureChatCompletion()
    {



        AzureChatCompletion azureChatCompletion = new(
            TestConfiguration.AzureOpenAI.ChatDeploymentName,
            TestConfiguration.AzureOpenAI.Endpoint,
            CredentialHelper.GetCredential());
        return azureChatCompletion;
    }



    public static IKernel GetKernelBuilder()
    {
        var authOptions = new DefaultAzureCredentialOptions
        {
            ExcludeEnvironmentCredential = true,
            ExcludeManagedIdentityCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeAzureCliCredential = false,
            ExcludeVisualStudioCredential = false,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeInteractiveBrowserCredential = true,
        };



        IKernel kernel = new KernelBuilder()
            .WithLoggerFactory(ConsoleLogger.LoggerFactory)
            // Add Azure chat completion service using DefaultAzureCredential AAD auth
            .WithAzureChatCompletionService(
                TestConfiguration.AzureOpenAI.ChatDeploymentName,
                TestConfiguration.AzureOpenAI.Endpoint,
                new DefaultAzureCredential(authOptions))
            .WithAzureTextCompletionService(
                           TestConfiguration.AzureOpenAI.ChatDeploymentName,
                                          TestConfiguration.AzureOpenAI.Endpoint,
                                                         new DefaultAzureCredential(authOptions))
            .Build();



        return kernel;
    }
}
