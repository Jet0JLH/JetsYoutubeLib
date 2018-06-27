Public Class channelStatistics
    Protected _channelID As String
    Protected _apiKey As String
    Protected _subscriberCount As Integer
    Protected _channelViews As Integer
    Protected _videoCount As Integer
    Protected _lastUpdateTime As Date
    Protected webReq As Net.WebRequest
    Protected webStream As IO.Stream
    Public Sub New(ApiKey As String, ChannelID As String, Optional doUpdate As Boolean = False)
        Me.New
        Me.apiKey = ApiKey
        Me.channelID = ChannelID
        If doUpdate Then
            Update()
        End If
    End Sub
    Public Sub New()
        _apiKey = ""
        _channelID = ""
        _subscriberCount = 0
        _channelViews = 0
        _videoCount = 0
    End Sub
    Public Property channelID As String
        Get
            Return _channelID
        End Get
        Set(value As String)
            _channelID = value
        End Set
    End Property
    Public Property apiKey As String
        Get
            Return _apiKey
        End Get
        Set(value As String)
            _apiKey = value
        End Set
    End Property
    Public ReadOnly Property subscriberCount As Integer
        Get
            Return _subscriberCount
        End Get
    End Property
    Public ReadOnly Property channelViews
        Get
            Return _channelViews
        End Get
    End Property
    Public ReadOnly Property videoCount As Integer
        Get
            Return _videoCount
        End Get
    End Property
    Public ReadOnly Property lastUpdateTime As Date
        Get
            Return _lastUpdateTime
        End Get
    End Property
    Public Sub update()
        'Do UpdateStuff
        Try
            Dim apiString As String = "https://www.googleapis.com/youtube/v3/channels?part=statistics&id=" & channelID & "&key=" & apiKey
            webReq = Net.WebRequest.Create(apiString)
            webStream = webReq.GetResponse.GetResponseStream
            Dim objReader As New IO.StreamReader(webStream)
            Dim content As String = objReader.ReadToEnd
            Dim jsonObj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(content)
            With jsonObj.SelectToken("items").First.SelectToken("statistics")
                _channelViews = .Value(Of Integer)("viewCount")
                _subscriberCount = .Value(Of Integer)("subscriberCount")
                _videoCount = .Value(Of Integer)("videoCount")
            End With
            _lastUpdateTime = My.Computer.Clock.LocalTime
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
