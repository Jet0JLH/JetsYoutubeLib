Public Class channelStatistics
    Protected _channelID As String
    Protected _apiKey As String
    Protected _subscriberCount As Integer
    Protected _channelViews As Integer
    Protected _videoCount As Integer
    Protected _channelName As String
    Protected _channelDescription As String
    Protected _country As String
    Protected _hiddenSubscriberCount As Boolean
    Protected _channelCreationDate As Date
    Protected _lastUpdateTime As Date
    Protected webReq As Net.WebRequest
    Protected webStream As IO.Stream

    Public Event subscriberCountChanged(ByRef sender As Object, ByVal oldValue As Integer, ByVal newValue As Integer)
    Public Event channelViewsChanged(ByRef sender As Object, ByVal oldValue As Integer, ByVal newValue As Integer)
    Public Event videoCountChanged(ByRef sender As Object, ByVal oldValue As Integer, ByVal newValue As Integer)
    Public Event channelDescriptionChanged(ByRef sender As Object, ByVal oldValue As String, ByVal newValue As String)
    Public Event hiddenSubscriberCountChanged(ByRef sender As Object, ByVal oldValue As Boolean, ByVal newValue As Boolean)
    Public Event updated(ByRef sender As Object, ByVal lastUpdateTime As Date, ByVal newUpdateTime As Date)

    Public Sub New(ApiKey As String, ChannelID As String, Optional doFullUpdate As Boolean = False)
        Me.New
        Me.apiKey = ApiKey
        Me.channelID = ChannelID
        If doFullUpdate Then
            update()
        End If
    End Sub
    Public Sub New()
        _apiKey = ""
        _channelID = ""
        _subscriberCount = 0
        _channelViews = 0
        _videoCount = 0
        _channelName = ""
        _channelDescription = ""
        _country = "N/A"
        _hiddenSubscriberCount = False
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
    Public ReadOnly Property channelName As String
        Get
            Return _channelName
        End Get
    End Property
    Public ReadOnly Property channelDescription As String
        Get
            Return _channelDescription
        End Get
    End Property
    Public ReadOnly Property country As String
        Get
            Return _country
        End Get
    End Property
    Public ReadOnly Property hiddenSubscriberCount As Boolean
        Get
            Return _hiddenSubscriberCount
        End Get
    End Property
    Public ReadOnly Property channelCreationDate As Date
        Get
            Return _channelCreationDate
        End Get
    End Property
    Public ReadOnly Property lastUpdateTime As Date
        Get
            Return _lastUpdateTime
        End Get
    End Property
    Public Sub update()
        If My.Computer.Network.IsAvailable Then
            If My.Computer.Network.Ping("www.googleapis.com") = False Then
                Throw New System.Exception("www.googleapis.com is not available")
                Exit Sub
            End If
        Else
                Throw New System.Exception("No network available")
            Exit Sub
        End If
        Try
            Dim apiString As String = "https://www.googleapis.com/youtube/v3/channels?part=statistics,snippet&id=" & channelID & "&key=" & apiKey
            webReq = Net.WebRequest.Create(apiString)
            webStream = webReq.GetResponse.GetResponseStream
            Dim objReader As New IO.StreamReader(webStream)
            Dim content As String = objReader.ReadToEnd
            Dim jsonObj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(content)
            With jsonObj.SelectToken("items").First.SelectToken("snippet")
                Dim tempString As String = ""
                _channelName = .Value(Of String)("title")
                tempString = .Value(Of String)("description")
                If _channelDescription <> tempString Then
                    RaiseEvent channelDescriptionChanged(Me, _channelDescription, tempString)
                    _channelDescription = tempString
                End If
                _country = .Value(Of String)("country")
                _channelCreationDate = Date.Parse(.Value(Of String)("publishedAt"))
            End With
            With jsonObj.SelectToken("items").First.SelectToken("statistics")
                Dim tempInt As Integer = 0
                tempInt = .Value(Of Integer)("viewCount")
                If _channelViews <> tempInt Then
                    RaiseEvent channelViewsChanged(Me, channelViews, tempInt)
                    _channelViews = tempInt
                End If
                tempInt = .Value(Of Integer)("subscriberCount")
                If _subscriberCount <> tempInt Then
                    RaiseEvent subscriberCountChanged(Me, _subscriberCount, tempInt)
                    _subscriberCount = tempInt
                End If
                tempInt = .Value(Of Integer)("videoCount")
                If _videoCount <> tempInt Then
                    RaiseEvent videoCountChanged(Me, _videoCount, tempInt)
                    _videoCount = tempInt
                End If
                Dim tempBool As Boolean = .Value(Of Boolean)("hiddenSubscriberCount")
                If _hiddenSubscriberCount <> tempBool Then
                    RaiseEvent hiddenSubscriberCountChanged(Me, _hiddenSubscriberCount, tempBool)
                    _hiddenSubscriberCount = tempBool
                End If
            End With
            Dim tempNewTime As Date = My.Computer.Clock.LocalTime
            RaiseEvent updated(Me, _lastUpdateTime, tempNewTime)
            _lastUpdateTime = tempNewTime
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
