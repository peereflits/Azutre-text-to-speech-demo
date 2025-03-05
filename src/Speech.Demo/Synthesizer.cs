using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Speech.Demo;

internal interface ISynthesize
{
    Task<byte[]> AudioAsync(string ssml);
}

internal class Synthesizer
(
    AppSettings settings,
    ILogger logger
) : ISynthesize
{
    public async Task<byte[]> AudioAsync(string ssml)
    {
        logger.Log("Synthesizing ...");

        var message = string.Empty;

        var speechConfig = BuildSpeechConfig();

        using var synthesizer = new SpeechSynthesizer(speechConfig/*, null*/);
        using var result = await synthesizer.SpeakSsmlAsync(ssml);

        switch (result.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                logger.Log("Synthesized.");
                break;

            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                logger.Warn($"Canceled synthesizing. Reason: {cancellation.Reason}");
                message += $"Failed to synthesize due to {cancellation.Reason}";

                if (cancellation.Reason == CancellationReason.Error)
                {
                    logger.Error("Canceled synthesizing.");
                    logger.Error($"Canceled: ErrorCode={cancellation.ErrorCode}");
                    logger.Error($"Canceled: ErrorDetails={cancellation.ErrorDetails}");
                    logger.Error("Canceled: Did you update the subscription info?");

                    message += $"\r\nErrorCode={cancellation.ErrorCode};ErrorDetails={cancellation.ErrorDetails}";
                }

                throw new DemoException(message);
        }

        return result.AudioData;
    }

    private SpeechConfig BuildSpeechConfig()
    {
        var speechConfig = SpeechConfig.FromSubscription(settings.SpeechKey, settings.SpeechLocation);
        //speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz64KBitRateMonoMp3);
        return speechConfig;
    }
}