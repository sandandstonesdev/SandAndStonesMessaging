using SandAndStonesMessaging.BashError;
using SandAndStonesMessaging.Codec;
using SandAndStonesMessaging.Messages;
using SandAndStonesMessaging.Utilities;

Console.WriteLine("Encoding/Decoding Message Util");

BashErrorBase bashErrorCode = new BashError();
try
{
    Dictionary<string, string> headersDictionary = GenerateHeadersDictionary();
    byte[] payloadBytes = GeneratePayloadBytes();

    DisplayInputMessage(headersDictionary, payloadBytes);

    IMessageCodec codec = new MessageCodec();
    byte[] encodedMessageBytes = DisplayEncodedMessage(codec, headersDictionary, payloadBytes);

    IMessage outputMessage = codec.Decode(encodedMessageBytes);
    DisplayDecodedMessage(outputMessage);
    bashErrorCode = BashError.GetNoError();

    //Uncomment to see error handling
    //throw new Exception("Fatal error!");

}
catch (Exception generalException) // Type simplified
{
    Console.WriteLine($"Error occuured:{System.Environment.NewLine}{generalException.Message}");
    Console.WriteLine($"Simplified callstack:{System.Environment.NewLine}{generalException.StackTrace}");
    bashErrorCode = BashError.GetFatalError();
}
finally
{
    Console.WriteLine("Program end!");
    Console.WriteLine($"Error code returned to OS bash: { bashErrorCode.Get()}");
    Console.ReadLine();
    
}

return bashErrorCode.Get();

Dictionary<string, string> GenerateHeadersDictionary()
{
    Dictionary<string, string> headersDictionary = new()
            {
                { "headerX", "valueX" },
                { "headerY", "valueY" }
            };
    return headersDictionary;
}

byte[] GeneratePayloadBytes()
{
    byte[] payloadBytes = new byte[8];
    Random rnd = new();
    for (int i = 0; i < payloadBytes.Length; i++)
    {
        payloadBytes[i] = (byte)(rnd.Next() % 255);
    }
    return payloadBytes;
}

void DisplayInputMessage(Dictionary<string, string> headersDictionary, byte[] payloadBytes)
{
    Console.WriteLine(Utils.ConsoleSeparator);
    Console.WriteLine("Input message:");
    foreach (var header in headersDictionary)
    {
        Console.WriteLine($"Headers: {header.Key} : {header.Value}");
    }

    string hexPayloadInput = BitConverter.ToString(payloadBytes);
    Console.WriteLine($"Input message bytes: {hexPayloadInput}");
    Console.WriteLine(Utils.ConsoleSeparator);

}

byte[] DisplayEncodedMessage(IMessageCodec codec, Dictionary<string, string> headersDictionary, byte[] payloadBytes)
{
    Console.WriteLine("Encoded message:");
    IMessage inputMessage = new Message(headersDictionary, payloadBytes);

    byte[] encodedMessageBytes = codec.Encode(inputMessage);

    string hex = BitConverter.ToString(encodedMessageBytes);
    Console.WriteLine($"Encoded message bytes: {hex}");
    Console.WriteLine(Utils.ConsoleSeparator);

    return encodedMessageBytes;
}

void DisplayDecodedMessage(IMessage encodedMessage)
{
    Console.WriteLine("Output message:");
    Console.WriteLine("Decoded message:");
    foreach (var header in encodedMessage.Headers)
    {
        Console.WriteLine($"Headers: {header.Key} : {header.Value}");
    }

    string hexPayload = BitConverter.ToString(encodedMessage.Payload);
    Console.WriteLine($"Payload {hexPayload}");
    Console.WriteLine(Utils.ConsoleSeparator);

}