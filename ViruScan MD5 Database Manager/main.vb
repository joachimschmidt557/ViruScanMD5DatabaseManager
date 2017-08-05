Option Compare Text

Imports System.IO
Imports System.Text

Public Class main

    Private Sub NewDatabaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewDatabaseToolStripMenuItem.Click
        NewDatabase()
    End Sub

    Private Sub OpenDatabaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenDatabaseToolStripMenuItem.Click
        OpenDatabase()
    End Sub

    Private Sub SaveDatabaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveDatabaseToolStripMenuItem.Click
        SaveDatabase()
    End Sub

    Private Sub ImportToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ImportToolStripMenuItem.Click
        Import()
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportToolStripMenuItem.Click
        Export()
    End Sub

    Private Sub AddDataToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddDataToolStripMenuItem.Click
        Add()
    End Sub

    Private Sub RemoveDataToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RemoveDataToolStripMenuItem.Click
        Remove()
    End Sub

    Private Sub NewToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripButton.Click
        NewDatabase()
    End Sub

    Private Sub OpenToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripButton.Click
        OpenDatabase()
    End Sub

    Private Sub SaveToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripButton.Click
        SaveDatabase()
    End Sub

    Private Sub HelpToolStripButton_Click(sender As System.Object, e As System.EventArgs)
        '!!!!!!!!!!!!!!!!!!!!!!!!!!
    End Sub

    Private Sub AddToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles AddToolStripButton.Click
        Add()
    End Sub

    Private Sub RemoveToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles RemoveToolStripButton.Click
        Remove()
    End Sub

    Private Sub ImportToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles ImportToolStripButton.Click
        Import()
    End Sub

    Private Sub ExportToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles ExportToolStripButton.Click
        Export()
    End Sub



    Private Sub Import()
        If ImportFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Status.Text = "Importing"
                Dim text As String()
                text = File.ReadAllLines(ImportFileDialog.FileName)
                ParseRare(text)
                Status.Text = "Ready"
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub Export()
        If ExportFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Status.Text = "Exporting"
                Dim text(0 To ListView.Items.Count - 1) As String
                Dim i As Integer
                For Each item As ListViewItem In ListView.Items
                    text(i) = item.SubItems(2).Text
                    i += 1
                Next
                'If Not File.Exists(ExportFileDialog.FileName) Then File.Create(ExportFileDialog.FileName)
                File.WriteAllLines(ExportFileDialog.FileName, text)
                Status.Text = "Ready"
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub Remove()
        If ListView.SelectedItems.Count > 0 Then
            For Each item As ListViewItem In ListView.SelectedItems
                item.Remove()
            Next
        End If
    End Sub

    Private Sub Add()
        DialogEdit.TypeTextBox.Text = ""
        DialogEdit.NameTextBox.Text = ""
        DialogEdit.HashTextBox.Text = ""
        If DialogEdit.ShowDialog = Windows.Forms.DialogResult.OK Then
            If DialogEdit.HashTextBox.Text.Length = 32 Then
                ListView.Items.Add(DialogEdit.TypeTextBox.Text)
                ListView.Items(ListView.Items.Count - 1).SubItems.Add(DialogEdit.NameTextBox.Text)
                ListView.Items(ListView.Items.Count - 1).SubItems.Add(DialogEdit.HashTextBox.Text)
            Else
                MsgBox("Not a valid MD5 hash!")
            End If
        End If
    End Sub

    Private Sub Edit()
        If ListView.SelectedItems.Count = 1 Then
            'One item selected
            DialogEdit.TypeTextBox.Text = ListView.SelectedItems(0).Text
            DialogEdit.NameTextBox.Text = ListView.SelectedItems(0).SubItems(1).Text
            DialogEdit.HashTextBox.Text = ListView.SelectedItems(0).SubItems(2).Text
            If DialogEdit.ShowDialog = Windows.Forms.DialogResult.OK Then
                ListView.SelectedItems(0).Text = DialogEdit.TypeTextBox.Text
                ListView.SelectedItems(0).SubItems(1).Text = DialogEdit.NameTextBox.Text
            End If
        ElseIf ListView.SelectedItems.Count > 1 Then
            'Multiple items seleted
            DialogEdit.TypeTextBox.Text = ""
            DialogEdit.NameTextBox.Text = ""
            For Each item As ListViewItem In ListView.SelectedItems
                DialogEdit.HashTextBox.AppendText(item.SubItems(2).Text + vbCrLf)
            Next
            If DialogEdit.ShowDialog = Windows.Forms.DialogResult.OK Then
                For Each item As ListViewItem In ListView.SelectedItems
                    item.Text = DialogEdit.TypeTextBox.Text
                    item.SubItems(1).Text = DialogEdit.NameTextBox.Text
                Next
            End If
        End If
    End Sub

    Private Sub NewDatabase()
        ListView.Items.Clear()
    End Sub

    Private Sub OpenDatabase()
        If OpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Status.Text = "Loading database"
                Dim text As String()
                text = File.ReadAllLines(OpenFileDialog.FileName)
                Parse(text)
                Status.Text = "Ready"
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub SaveDatabase()
        If SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Status.Text = "Saving database"
                Dim text(0 To ListView.Items.Count - 1) As String
                Dim i As Integer
                For Each item As ListViewItem In ListView.Items
                    Dim temp As New StringBuilder
                    temp.Append(item.Text)
                    temp.Append("|")
                    temp.Append(item.SubItems(1).Text)
                    temp.Append("|")
                    temp.Append(item.SubItems(2).Text)
                    text(i) = temp.ToString()
                    i += 1
                Next
                'If Not File.Exists(SaveFileDialog.FileName) Then File.Create(SaveFileDialog.FileName)
                File.WriteAllLines(SaveFileDialog.FileName, text)
                Status.Text = "Ready"
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim thisProc As Process = Process.GetCurrentProcess
        thisProc.PriorityBoostEnabled = True
        thisProc.PriorityClass = ProcessPriorityClass.AboveNormal
        Status.Text = "Ready; No database loaded"
    End Sub

    Private Sub Parse(ByVal text As String())
        For Each value As String In text
            Try
                If value Like ";*" Then
                ElseIf value Like "[#]*" Then
                Else
                    Try
                        Dim splitted() As String = value.Split("|")
                        ListView.Items.Add(splitted(0))
                        ListView.Items(ListView.Items.Count - 1).SubItems.Add(splitted(1))
                        ListView.Items(ListView.Items.Count - 1).SubItems.Add(splitted(2))
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Next
    End Sub

    Private Sub ParseRare(ByVal text As String())
        For Each value As String In text
            Try
                If value Like "[#]*" Then
                ElseIf value Like ";*" Then
                Else
                    ListView.Items.Add("")
                    ListView.Items(ListView.Items.Count - 1).SubItems.Add("")
                    ListView.Items(ListView.Items.Count - 1).SubItems.Add(value.ToUpper)
                End If
            Catch ex As Exception

            End Try
        Next
    End Sub

    Private Sub Write()
        'not needed
    End Sub

    Private Sub WriteRare()
        'Not needed
    End Sub

    Private Sub EditToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles EditToolStripButton.Click
        Edit()
    End Sub

    Private Sub EditDataToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditDataToolStripMenuItem.Click
        Edit()
    End Sub

    Private Sub FindToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles FindToolStripButton.Click
        Dim i As Integer = 0
        While (i < ListView.Items.Count)
            RemoveListViewLine(i, ListView.Items(i).SubItems(2).Text)
            i = i + 1
        End While
    End Sub

    Private Sub RemoveListViewLine(ByVal n As Integer, ByVal TextCrit As String)
        Dim li As ListViewItem
        n = n + 1
        While (n < ListView.Items.Count)
            li = ListView.Items(n)
            If li.SubItems(2).Text = TextCrit Then
                ListView.Items.Remove(li)
            Else
                n = n + 1
            End If
        End While
    End Sub

    Private Sub CountToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles CountToolStripButton.Click
        MsgBox(ListView.Items.Count.ToString)
    End Sub
End Class
