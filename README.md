# SandAndStonesMessagingSample

Format of Message as binary packet:

	---
	Packet Signature Magic = 'SAND' - first 4 bytes
	---
	Head Section Signature Magic 'HEAD'
	Packet Header CRC
	Headers Count
	Header1: HeaderName + 0x20 (CR) + HeaderValue + 0x0D (Space)
	...
	HeaderN: HeaderName + 0x20 (CR) + HeaderValue + 0x0D (Space)
	----
	Payload Section Signature Magic 'PAYL'
	Packet Payload CRC
	Payload Bytes Count
	Payload
	----
	End of Packet Signature Magic = 'ENDP' - last 4 bytes
	----
	
# Docker instructions

Run build, run tests and run app in docker:

1. docker build -t sandmsgapp .
2. docker run --rm -v ${pwd}:/src -w /src/SandAndStonesMessagingTests mcr.microsoft.com/dotnet/sdk:7.0 dotnet test
3. docker run sandmsgapp

## Author
- [Paweł Aksiutowicz](https://github.com/sandandstonesdev)