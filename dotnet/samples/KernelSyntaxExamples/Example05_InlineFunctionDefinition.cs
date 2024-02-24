// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.SemanticKernel;
using RepoUtils;

// ReSharper disable once InconsistentNaming
public static class Example05_InlineFunctionDefinition
{
    public static async Task RunAsync()
    {
        Console.WriteLine("======== Inline Function Definition ========");

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
            .Build();

        // Function defined using few-shot design pattern
        const string FunctionDefinition = @"
Generate a creative reason or excuse for the given event.
Be creative and be funny. Let your imagination run wild.

Event: I am running late.
Excuse: I was being held ransom by giraffe gangsters.

Event: I haven't been to the gym for a year
Excuse: I've been too busy training my pet dragon.

Event: {{$input}}
";

        var excuseFunction = kernel.CreateSemanticFunction(FunctionDefinition, maxTokens: 100, temperature: 0.4, topP: 1);

        var result = await excuseFunction.InvokeAsync("I missed the F1 final race");
        Console.WriteLine(result);

        result = await excuseFunction.InvokeAsync("sorry I forgot your birthday");
        Console.WriteLine(result);

        var fixedFunction = kernel.CreateSemanticFunction($"Translate this date {DateTimeOffset.Now:f} to French format", maxTokens: 100);

        result = await fixedFunction.InvokeAsync();
        Console.WriteLine(result);
    }
}
