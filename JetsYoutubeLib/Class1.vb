Public Class JetsYoutubeLib
    Public Class statistics
        Protected _channelID As String
        Protected _ApiKey As String
        Protected _SubscriberCount As Integer
        Protected _ChannelViews As Integer
        Protected _VideoCount As Integer
        Protected _lastUpdateTime As Date
        Protected webReq As Net.WebRequest
        Protected webStream As IO.Stream
        Public Sub New(ApiKey As String, ChannelID As String, Optional doUpdate As Boolean = False)
            Me.ApiKey = ApiKey
            Me.ChannelID = ChannelID
            If doUpdate Then
                Update()
            End If
        End Sub
        Public Property ChannelID As String
            Get
                Return _channelID
            End Get
            Set(value As String)
                _channelID = value
            End Set
        End Property
        Public Property ApiKey As String
            Get
                Return _ApiKey
            End Get
            Set(value As String)
                _ApiKey = value
            End Set
        End Property
        Public ReadOnly Property SubscriberCount As Integer
            Get
                Return _SubscriberCount
            End Get
        End Property
        Public ReadOnly Property ChannelViews
            Get
                Return _ChannelViews
            End Get
        End Property
        Public ReadOnly Property VideoCount As Integer
            Get
                Return _VideoCount
            End Get
        End Property
        Public ReadOnly Property lastUpdateTime As Date
            Get
                Return _lastUpdateTime
            End Get
        End Property
        Public Sub Update()
            'Do UpdateStuff
            Try
                Dim apiString As String = "https://www.googleapis.com/youtube/v3/channels?part=statistics&id=" & ChannelID & "&key=" & ApiKey
                webReq = Net.WebRequest.Create(apiString)
                webStream = webReq.GetResponse.GetResponseStream
                Dim objReader As New IO.StreamReader(webStream)
                Dim content As String = objReader.ReadToEnd
                Dim jsonObj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(content)
                With jsonObj.SelectToken("items").First.SelectToken("statistics")
                    _ChannelViews = .Value(Of Integer)("viewCount")
                    _SubscriberCount = .Value(Of Integer)("subscriberCount")
                    _VideoCount = .Value(Of Integer)("videoCount")
                End With
                _lastUpdateTime = My.Computer.Clock.LocalTime
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub
    End Class
End Class
