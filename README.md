# JetsYoutubeLib

## Description
This libary is for reading statistics of a youtube channel. Requirements for using is .Net Framework 4.5

## How to use
1. Download JetsYoutubeLib.dll and if necessary also the Newtonsoft.Json.dll
2. Create a reference to JetsYoutubeLib.dll and Newtonsoft.Json.dll
3. Import JetsYoutubeLib
4. Create a new object like this example:
	Dim channelInfo As New JetsYoutubeLib.channelStatistics("your-API-Key", "ChannelID", true)
5. Now you can read infos about the channel. As example the subscription count
	Console.writeLine(channelInfo.subscriberCount)