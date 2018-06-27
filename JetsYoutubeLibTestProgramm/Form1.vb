Public Class Form1
    Dim channelInfo As New JetsYoutubeLib.channelStatistics
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then
            channelInfo.apiKey = TextBox1.Text
            channelInfo.channelID = TextBox2.Text
            channelInfo.update()
            Label6.Text = channelInfo.channelViews
            Label7.Text = channelInfo.subscriberCount
            Label8.Text = channelInfo.videoCount
        Else
            MsgBox("Please insert API Key and Channel ID first", MsgBoxStyle.Critical)
        End If
    End Sub
End Class
