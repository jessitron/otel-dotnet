# Into to Observability: OpenTelemetry in .NET

This ASP.NET application is here for you to try out tracing.
It consists of a microservice that calls itself, so you can simulate
a whole microservice ecosystem with just one service!

## What to do

You can remix this app on Glitch or [clone the repo](https://github.com/jessitron/otel-dotnet) and open it in your IDE.

### 1. Autoinstrument!

This tracing happens with only one code change!

See the "Tracing!" comment in `Startup.cs`. That is where OpenTelemetry instrumentation is set up.

You'll see the web requests coming in. They'll even nest inside each other when the service calls itself. You will not yet
see information that is special to this app, like the query parameter on the request.

#### Configure the connection to Honeycom

In `.env` in glitch or your run configuration in your IDE, add these
environment variables:

```
HONEYCOMB_API_KEY=replace-this-with-a-real-api-key
HONEYCOMB_DATASET=otel-dotnet
SERVICE_NAME=fibonacci-microservice
SAMPLE_RATE=1
```

Get a Honeycomb API Key from your Team Settings in [Honeycomb](https://ui.honeycomb.io).
(find this by clicking on your profile in the lower-left corner.)

You can name the Honeycomb Dataset anything you want.

You can choose any Service Name you want.

The Sample Rate determines how many requests each saved trace represents; 1 means "keep all of them." Right now you want all of them.

#### See the results

Run the app. Activate the sequence of numbers.
Go to [Honeycomb](https://ui.honeycomb.io) and choose the Dataset you configured.

How many traces are there?

How many spans are in the traces?

Why are there so many??

Which trace has the most, and why is it different?

## 2. Customize a span

Let's make it easier to see what the "index" query parameter is.

You don't even have to access OpenTelemetry directly to do this.
You can use the built-in .NET concept of Activity.

In `FibonacciController.cs`, add the index parameter as a custom attribute like this:

`System.Diagnostics.Activity.Current.AddTag("parameter.index", iv.ToString());`

Restart the app, make the sequence go, and find that field on the new spans.

Can you make the trace waterfall view show the index? What pattern does it show?

## 3. Create a custom span

Make the calculation into its own span, to see how much of the time spent on
this service is the meat: adding the fibonacci numbers.

Break out a method for creating the returned Fibonacci number, and ...?

After a restart, do your traces show this extra span? Do you see the name of your method?
What percentage of the service time is spend in it?

