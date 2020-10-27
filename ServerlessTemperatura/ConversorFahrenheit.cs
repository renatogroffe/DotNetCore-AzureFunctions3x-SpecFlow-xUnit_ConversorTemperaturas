using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ServerlessTemperatura
{
    public static class ConversorFahrenheit
    {
        private static readonly CultureInfo _CULTURE = new CultureInfo("en-US");

        [FunctionName("ConversorFahrenheit")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Acionada a Function ConversorFahrenheit - HTTP Trigger");

            string strTempFahrenheit = req.Query["tempFahrenheit"];
            if (!String.IsNullOrWhiteSpace(strTempFahrenheit) &&
                double.TryParse(strTempFahrenheit, out _))
            {
                log.LogInformation(
                    $"Recebida temperatura para conversão: {strTempFahrenheit}");
                double tempFahrenheit = Convert.ToDouble(
                    strTempFahrenheit, _CULTURE.NumberFormat);
                if (tempFahrenheit >= -459.67) // Temperatura maior ou igual ao zero absoluto
                    return new OkObjectResult(
                        new Temperatura(tempFahrenheit));
            }

            log.LogError(
                $"Informe uma temperatura válida! Valor recebido: {strTempFahrenheit}");
            return new BadRequestObjectResult(
                new
                {
                    Sucesso = false,
                    Mensagem = "Preencha o parâmetro 'tempFahrenheit' com uma temperatura válida!"
                });
        }
    }
}