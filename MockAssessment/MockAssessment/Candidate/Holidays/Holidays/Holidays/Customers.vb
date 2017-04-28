Imports System.IO

Public Class Customers
    Private Structure Customer
        Public ID As String
        Public FirstName As String
        Public LastName As String
        Public Email As String                  'Creating the structure that will hold the  data.
        Public PhoneNumber As String
        Public DOB As String
    End Structure

    Public Function Wipe() 'this will clear any text from within the textboxes
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtEmail.Text = ""
        txtPhoneNumber.Text = ""
    End Function

    Public Function ShowControls() 'this will show the controls to the user when they need to enter data
        lblFirstName.Visible = True
        lblLastName.Visible = True
        lblEmail.Visible = True
        lblPhoneNumber.Visible = True
        lblDOB.Visible = True
        txtFirstName.Visible = True
        txtLastName.Visible = True
        txtEmail.Visible = True
        txtPhoneNumber.Visible = True
        dtpDOB.Visible = True
    End Function

    Public Function hideControls() 'Likewise, this will hide the controls from the user
        lblFirstName.Visible = False
        lblLastName.Visible = False
        lblEmail.Visible = False
        lblPhoneNumber.Visible = False
        lblDOB.Visible = False
        txtFirstName.Visible = False
        txtLastName.Visible = False
        txtEmail.Visible = False
        txtPhoneNumber.Visible = False
        dtpDOB.Visible = False
    End Function

    Private Sub Holidays_Load() Handles MyBase.Load
        If Dir$("customerdetails.txt") = "" Then
            Dim sw As New StreamWriter("customerdetails.txt", True)    'This makes sure there is actually a database to enter/read data. If not, it creates a new blank one.
            sw.WriteLine("")
            sw.Close()
            MsgBox("A new file has been created", vbExclamation, "Warning!")
        End If

        Dim ReadLines As String() = File.ReadAllLines(Dir$("customerdetails.txt")) 'This will make sure that the Client ID is correct by adding one to what it last finds in the text file
        txtID.Text = Val(Trim(Mid(ReadLines.Last, 1, 20))) + 1

        txtID.Enabled = False
        cmdNewSearch.Visible = False
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If txtFirstName.Text = "" Then 'This will be where I do my validation
            MessageBox.Show("Please enter your first name.") 'this will test if there is any text entered into a textbox
            Exit Sub
        ElseIf txtLastName.Text = "" Then
            MessageBox.Show("Please enter your last name")
            Exit Sub
        ElseIf txtEmail.Text = "" Then
            MessageBox.Show("Please enter your email.")
            Exit Sub
        ElseIf txtPhoneNumber.TextLength = 0 Then
            MessageBox.Show("Please enter your phone number.")
            Exit Sub
        ElseIf txtPhoneNumber.TextLength <> 11 Then 'this will test the length of text within the textbox to see if it meets the requirement, if not, it gives an error
            MessageBox.Show("Invalid phone number")
            Exit Sub
        ElseIf dtpDOB.Value.ToString("yyyy") = 2015 Then
            MessageBox.Show("Year date is higher than 2015")
            Exit Sub
        ElseIf txtEmail.Text.Contains("@") = False Then 'this will test to see if the user's email address contains an @ symbol.
            MessageBox.Show("Please enter a vaild email address")
            Exit Sub
        End If

        If txtLastName.TextLength > 15 Then 'If the text is longer than the length allowed, it will shorten it to its specified amount
            MessageBox.Show("Your last name is too long and has been shorten to 15 characters")
        ElseIf txtFirstName.TextLength > 12 Then
            MessageBox.Show("Your first name is too long and has been shorten to 12 characters")
        End If

        Dim CustomerData As New Customer
        Dim DOB As String
        Dim sw As New System.IO.StreamWriter("customerdetails.txt", True)
        CustomerData.ID = LSet(txtID.Text, 20)
        CustomerData.FirstName = LSet(txtFirstName.Text, 12)
        CustomerData.LastName = LSet(txtLastName.Text, 15)
        CustomerData.Email = LSet(txtEmail.Text, 35)                      'Filling the structure with data.
        CustomerData.PhoneNumber = LSet(txtPhoneNumber.Text, 15)
        DOB = dtpDOB.Value.ToString("dd/MM/yyyy")
        CustomerData.DOB = LSet(DOB, 10)
        sw.WriteLine(CustomerData.ID & CustomerData.FirstName & CustomerData.LastName & CustomerData.Email & CustomerData.PhoneNumber & CustomerData.DOB)
        sw.Close()                                                                  'Always need to close afterwards
        MsgBox("File Saved!")
        Wipe()

        Dim NewID As Integer 'This will chance the Client ID everytime the save button is pressed.
        NewID = Val(txtID.Text) + 1
        txtID.Text = NewID
    End Sub

    Private Sub cmdSearch_Click(sender As System.Object, e As System.EventArgs) Handles cmdSearch.Click
        cmdNewSearch.Visible = True 'This will show another button which will look like the button has changed code
        hideControls()
        Wipe()
        txtID.Enabled = True
        txtID.Text = ""
    End Sub

    Private Sub cmdNewSearch_Click(sender As System.Object, e As System.EventArgs) Handles cmdNewSearch.Click
        If txtID.Text = "" Then
            MessageBox.Show("Please Enter the Client ID you are searching for")
        Else
            Dim ClientID As Integer
            ClientID = Val(txtID.Text) 'This will be what is used to search through the text document
            Dim CustomerData() As String = File.ReadAllLines(Dir$("customerdetails.txt"))
            Try
                For I = 0 To UBound(CustomerData)
                    If ClientID = Val(Trim(Mid(CustomerData(I), 1, 20))) Then 'if a integer with the same value of ClientID is found, then it will load the data from the text document
                        txtFirstName.Text = Trim(Mid(CustomerData(I), 21, 12))
                        txtLastName.Text = Trim(Mid(CustomerData(I), 33, 15))
                        txtEmail.Text = Trim(Mid(CustomerData(I), 48, 35))
                        txtPhoneNumber.Text = Trim(Mid(CustomerData(I), 83, 11))
                        dtpDOB.Value = CDate(Trim(Mid(CustomerData(I), 99, 10)))
                        ShowControls()
                        MessageBox.Show("Customer details have been found")
                        Exit Sub
                    End If
                Next
                MessageBox.Show("Failed to find the Client ID")
            Catch
                MessageBox.Show("Unable to load the Customer Details")
            End Try
        End If
    End Sub

    Private Sub txtFirstName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFirstName.KeyDown
        Select Case e.KeyCode 'By doing this, i am able to type check to make sure that no integers are entered into the first and last name text box
            Case Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.LControlKey
                SendKeys.Send("{Backspace}")
        End Select
    End Sub
    Private Sub txtLastName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLastName.KeyDown
        Select Case e.KeyCode
            Case Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.LControlKey
                SendKeys.Send("{Backspace}")
        End Select
    End Sub
End Class