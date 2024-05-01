// Michael Banko - CPS3330 SP24 Project
// Import necessary namespaces for the application
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Connect4; // Connect4 application namespace
using System;
using System.Net.Http;

// Create the WebAssembly host builder
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add the main application component to the builder
// The App component will be rendered in the HTML element with the id "#app"
builder.RootComponents.Add<App>("#app");

// Add the HeadOutlet component to the builder
// The HeadOutlet component will be rendered after the <head> section of the HTML document
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure the application to use an HTTP client with a base address of the host environment's base address
// This client can be used to make HTTP requests in your application
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Build and run the WebAssembly application asynchronously
await builder.Build().RunAsync();
