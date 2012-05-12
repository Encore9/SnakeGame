Public Class Form1

#Region "Structure/Variables"

    Structure Parts
        Dim x As Long
        Dim y As Long
        Dim facing As Integer
    End Structure
    Structure foodXY
        Dim x As Long
        Dim y As Long
    End Structure

    Dim snake(99) As Parts
    Dim length As Long
    Dim food As foodXY
    Dim i As Integer = 0

    Dim score As Integer
    Dim monster As Parts
    Dim iteration As Integer = 0
    Dim isgoing As Boolean = False
    Dim currentIT As Integer


    Dim SnakeQuad As Integer
    Dim MobQuad As Integer
    Dim SnakeSubQuad As Integer
    Dim MobSubQuad As Integer
    Dim designermode As Boolean = False
    Dim GameOver As Boolean = False
    Dim changepic As Boolean = True
    Dim music As Boolean = False
    Dim powerup As Integer
    Dim snakespeed As Integer = 90
    Dim monsterspeed As Integer = 90
#End Region

#Region "form"
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case 37
                snake(0).facing = 1 'Left
            Case 38
                snake(0).facing = 2 'Up
            Case 39
                snake(0).facing = 3 'Right
            Case 40
                snake(0).facing = 4 'Down
            Case Keys.P
                If GameOver = False Then
                    Timer1.Enabled = Not Timer1.Enabled
                    Timer2.Enabled = Not Timer2.Enabled
                End If
            Case Keys.R
                StartUp()
            Case Keys.H
                Label1.Visible = Not Label1.Visible
                Label2.Visible = Not Label2.Visible
                Label3.Visible = Not Label3.Visible
                Label4.Visible = Not Label4.Visible
                Label5.Visible = Not Label5.Visible
                Label6.Visible = Not Label6.Visible
                Label7.Visible = Not Label7.Visible
                Label8.Visible = Not Label8.Visible
                Label9.Visible = Not Label9.Visible
                Label10.Visible = Not Label10.Visible
                Label11.Visible = Not Label11.Visible
                Label12.Visible = Not Label12.Visible
                Label15.Visible = Not Label15.Visible
                Label16.Visible = Not Label16.Visible
                Label17.Visible = Not Label17.Visible
                Label18.Visible = Not Label18.Visible
                Label19.Visible = Not Label19.Visible
                Label13.Visible = Not Label13.Visible
                Label21.Visible = Not Label21.Visible
            Case Keys.D
                designermode = Not designermode
                Label14.Visible = Not Label14.Visible
            Case Keys.I
                If changepic = False Then
                    PictureBox1.Image = My.Resources.white
                    changepic = True
                Else
                    PictureBox1.Image = My.Resources.grid
                    changepic = False
                End If

            Case Keys.M
                music = Not music
                If music = True Then
                    Label22.Text = "Sound Status: Unmuted"
                Else
                    Label22.Text = "Sound Status: Muted"
                End If
            Case Keys.Escape
                End
        End Select
      
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.UpdateStyles()
        'My.Computer.Audio.Play("music\loop2.wav", AudioPlayMode.BackgroundLoop)
        Call StartUp()
    End Sub
#End Region

#Region "LOOP"
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        PictureBox1.Refresh()
        Call findfood()
        Call movesnake()
        Call findQuads()
        If designermode = False Then
            Call MonsterEat()
        End If

        'Call movemonster()

        If length <= 5 Then
            Timer1.Interval = 90
            Timer2.Interval = 90
        Else
            Timer1.Interval = snakespeed - (CInt(length * 0.7))
            Timer2.Interval = monsterspeed + (CInt(length * 0.5))
        End If

        
        Label1.Text = "snake.x " & snake(0).x
        Label2.Text = "snake.y " & snake(0).y
        Label3.Text = "food.x " & food.x
        Label4.Text = "food.y " & food.y
        Label5.Text = "score " & score
        Label6.Text = "length " & length
        Label7.Text = "itteration" & iteration
        Label8.Text = "monster.x " & monster.x
        Label9.Text = "monster.y " & monster.y
        Label10.Text = "monster.facing " & monster.facing
        Label15.Text = "in same quad " & isgoing
        iteration += 1
        Label11.Text = "mobquad " & MobQuad
        Label12.Text = "snakequad " & SnakeQuad
        Label13.Text = "timer1 interval " & Timer1.Interval

        Label16.Text = "currentIT " & currentIT
        Label18.Text = "snake sub quad " & SnakeSubQuad
        Label19.Text = "monster sub quad " & MobSubQuad
        Label21.Text = "timer2 interval " & Timer2.Interval
    End Sub
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Call findQuads()
        Call movemonster()
    End Sub
#End Region

#Region "place things"
    Private Sub StartUp()
        WriteText("")
        For d As Integer = 0 To 99
            snake(d).x = 0
            snake(d).y = 0
            snake(d).facing = 3
        Next
        food.x = 0
        food.y = 0
        score = 0
        monster.x = 0
        monster.y = 0
        iteration = 0
        isgoing = False
        currentIT = 0
        SnakeQuad = 0
        MobQuad = 0
        GameOver = False
        length = 5
        For a As Integer = 0 To 4
            snake(a).x = (120 - 10 * a)
            snake(a).y = 120
            Draw(snake(0).x, snake(0).y, Brushes.Blue)
            snake(a).facing = 3
        Next
        Randomize()
        PlaceObject(monster.x, monster.y)
        PlaceObject(food.x, food.y)
        Call findQuads()
        Timer1.Enabled = True
        Timer2.Enabled = True
        Label17.Visible = False
        Label17.Parent = PictureBox1
        Label17.BackColor = Color.Transparent
    End Sub

    Function PlaceObject(ByRef X As Integer, ByRef Y As Integer)
        Do
            X = CInt(Int(((PictureBox1.Width - 10) * Rnd()) + 10))
        Loop While (X Mod 10 <> 0)
        Do
            Y = CInt(Int(((PictureBox1.Height - 10) * Rnd()) + 10))
        Loop While (Y Mod 10 <> 0)
        Return Nothing
    End Function
#End Region

#Region "move things"

    Public Sub movemonster()
        If MobQuad = SnakeQuad Then

            
            Dim MobSubQ As Integer = MobQuad * 10
            Dim SnakeSubQ As Integer = SnakeQuad * 10
            Select Case MobSubQuad

                Case MobSubQ + 1
                    Select Case SnakeSubQuad
                        Case SnakeSubQ + 1
                            If isgoing = False Then
                                monster.facing = 0
                            End If


                        Case SnakeSubQ + 2
                            monster.facing = 6
                        Case SnakeSubQ + 3
                            monster.facing = 2
                        Case SnakeSubQ + 4
                            monster.facing = 3
                    End Select
                Case MobSubQ + 2
                    Select Case SnakeSubQuad
                        Case SnakeSubQ + 1
                            monster.facing = 4
                        Case SnakeSubQ + 2
                            If isgoing = False Then
                                monster.facing = 0
                            End If

                        Case SnakeSubQ + 3
                            monster.facing = 1
                        Case SnakeSubQ + 4
                            monster.facing = 2
                    End Select
                Case MobSubQ + 3
                    Select Case SnakeSubQuad
                        Case SnakeSubQ + 1
                            monster.facing = 8
                        Case SnakeSubQ + 2
                            monster.facing = 9
                        Case SnakeSubQ + 3
                            If isgoing = False Then
                                monster.facing = 0
                            End If

                        Case SnakeSubQ + 4
                            monster.facing = 6
                    End Select
                Case MobSubQ + 4

                    Select Case SnakeSubQuad
                        Case SnakeSubQ + 1
                            monster.facing = 7
                        Case SnakeSubQ + 2
                            monster.facing = 8
                        Case SnakeSubQ + 3
                            monster.facing = 4
                        Case SnakeSubQ + 4
                            If isgoing = False Then
                                monster.facing = 0
                            End If

                    End Select
            End Select

        Else
            isgoing = False
            Select Case MobQuad
                Case 1
                    Select Case SnakeQuad
                        Case 2
                            monster.facing = 6
                        Case 3
                            monster.facing = 2
                        Case 4
                            monster.facing = 3
                    End Select
                Case 2
                    Select Case SnakeQuad
                        Case 1
                            monster.facing = 4
                        Case 3
                            monster.facing = 1
                        Case 4
                            monster.facing = 2
                    End Select
                Case 3
                    Select Case SnakeQuad
                        Case 1
                            monster.facing = 8
                        Case 2
                            monster.facing = 9
                        Case 4
                            monster.facing = 6
                    End Select
                Case 4
                    Select Case SnakeQuad
                        Case 1
                            monster.facing = 7
                        Case 2
                            monster.facing = 8
                        Case 3
                            monster.facing = 4
                    End Select
            End Select
        End If

        Select Case monster.facing
            Case 0
                Do
                    monster.facing = CInt(Int((9 * Rnd()) + 1))
                Loop Until (monster.facing <> 5)
                isgoing = True
                currentIT = iteration
            Case 1
                monster.y = monster.y + 10 'moves down and to the left
                monster.x = monster.x - 10
            Case 2
                monster.y = monster.y + 10 'Moves down
            Case 3
                monster.y = monster.y + 10 'moves down and to the right
                monster.x = monster.x + 10
            Case 4
                monster.x = monster.x - 10 'Moves left
            Case 6
                monster.x = monster.x + 10 'Moves right
            Case 7
                monster.y = monster.y - 10 'moves up and the the left
                monster.x = monster.x - 10
            Case 8
                monster.y = monster.y - 10 'Moves up
            Case 9
                monster.y = monster.y - 10 'moves up and to the right
                monster.x = monster.x + 10
        End Select
        If currentIT + 10 = iteration Then
            isgoing = False
        End If

        'Draw the head and redraw the food because i refreshed the picturebox1
        Draw(snake(0).x, snake(0).y, Brushes.Blue)
        Draw(food.x, food.y, Brushes.Green)
        Draw(monster.x, monster.y, Brushes.Black)
    End Sub
    Private Sub movesnake()

        For i As Integer = (length - 1) To 1 Step (-1)
            snake(i).x = snake(i - 1).x 'sets the snake.x to the snake.x in front of it
            snake(i).y = snake(i - 1).y 'sets the snake.y  to the snake.y in front of it
            snake(i).facing = snake(i - 1).facing 'sets the snake.facing to the snake.facing in front of it
            If i = 0 Then
                Draw(snake(i).x, snake(i).y, Brushes.Blue)
            Else
                Draw(snake(i).x, snake(i).y, Brushes.Red)
            End If
            ' paints the snakepart
        Next i
        If snake(0).facing = 1 Then         'Moves left
            snake(0).x = snake(0).x - 10
        ElseIf snake(0).facing = 2 Then     'Moves up
            snake(0).y = snake(0).y - 10
        ElseIf snake(0).facing = 3 Then     'Moves right
            snake(0).x = snake(0).x + 10
        ElseIf snake(0).facing = 4 Then     'Moves down
            snake(0).y = snake(0).y + 10
        End If
        'Draw the head and redraw the food because i refreshed the picturebox1
        Draw(snake(0).x, snake(0).y, Brushes.Blue)
        Draw(food.x, food.y, Brushes.Green)
        Draw(monster.x, monster.y, Brushes.Black)
    End Sub
#End Region

#Region "QUADs"
    Private Sub findQuads()
        SnakeQuad = getQuad(snake(0).x, snake(0).y)
        MobQuad = getQuad(monster.x, monster.y)
        SnakeSubQuad = getSubQuad(SnakeQuad, snake(0).x, snake(0).y)
        MobSubQuad = getSubQuad(MobQuad, monster.x, monster.y)

    End Sub

    Function getQuad(ByVal X As Integer, ByVal Y As Integer)

        Dim quad As Integer = 1
        Select Case X
            Case Is < 300
                Select Case Y
                    Case Is < 300
                        quad = 1
                    Case Is >= 300
                        quad = 3
                End Select
            Case Is >= 300
                Select Case Y
                    Case Is < 300
                        quad = 2
                    Case Is >= 300
                        quad = 4
                End Select
        End Select
        Return quad
    End Function
    Function getSubQuad(ByVal Quad As Integer, ByVal X As Integer, ByVal Y As Integer)
        Dim SubQuad As Integer = 11

        SubQuad = Quad * 10 + 1
        If X >= (450 - (300 * (Quad Mod 2))) Then
            SubQuad = SubQuad + 1
        End If
        If Quad > 2 Then
            If Y >= 450 Then
                SubQuad = SubQuad + 2
            End If
        Else
            If Y >= 150 Then
                SubQuad = SubQuad + 2
            End If
        End If

        Return SubQuad
    End Function



#End Region

#Region "Collision Detection"
    Public Sub MonsterEat()
        If monster.x = snake(0).x And monster.y = snake(0).y Then
            'end game because you are dead
            GameIsOver()
        End If
        For c As Integer = 1 To length - 1
            If monster.x = snake(c).x And monster.y = snake(c).y Then
                'lose one length
                If length <= 3 Then
                    'game over
                    GameIsOver()
                Else
                    length -= 1
                End If

            End If
        Next

        If snake(0).x < 0 Or snake(0).x >= 600 Then
            'game over out of bounds
            GameIsOver()
        End If
        If snake(0).y < 0 Or snake(0).y >= 600 Then
            'game over out of bounds
            GameIsOver()
        End If
    End Sub
    Private Sub findfood()
        Dim timesUsed As Integer = 1
        If snake(0).x = food.x And snake(0).y = food.y Then
            If snake(length - 1).facing = 1 Then        'Looking left
                snake(length).x = snake(length - 1).x - 10
                snake(length).y = snake(length - 1).y
            ElseIf snake(length - 1).facing = 2 Then    'Looking up
                snake(length).x = snake(length - 1).x
                snake(length).y = snake(length - 1).y + 10
            ElseIf snake(length - 1).facing = 3 Then    'Looking right
                snake(length).x = snake(length - 1).x - 10
                snake(length).y = snake(length - 1).y
            ElseIf snake(length - 1).facing = 4 Then    'Looking down
                snake(length).x = snake(length - 1).x
                snake(length).y = snake(length - 1).y - 10
            End If
            If music = True Then
                My.Computer.Audio.Play(My.Resources.eat, AudioPlayMode.Background)
            End If
            Dim chance As Integer = CInt(Int((100 * Rnd()) + 1))
            If chance <= 50 Then
                powerup = CInt(Int((9 * Rnd()) + 1))
                If powerup <= 2 Then 'less than 2 (2/9 chance)
                    length += 5
                ElseIf powerup >= 3 And powerup <= 5 Then 'greater than 3 less than 5 (3/9 chance)
                    snakespeed = 90 - (timesUsed * 5)
                    timesUsed += 1
                ElseIf powerup >= 6 Then 'great than 6 less than 9 (4/9 chance)
                    length += 1
                End If


            End If
            'increase the length of the snake
            length = length + 1
            score += 100
            PlaceObject(food.x, food.y)
        End If
    End Sub
#End Region

#Region "MISC"

    Public Function Draw(ByVal x As Integer, ByVal y As Integer, ByVal color As System.Drawing.Brush)
        Dim g As Graphics
        g = PictureBox1.CreateGraphics
        g.FillRectangle(color, x, y, 10, 10)
        Return Nothing
    End Function
    Public Function WriteText(ByVal text As String)
        Label17.Text = text
        Dim lblcenterW As Integer = Label17.Width / 2
        Dim lblcenterH As Integer = Label17.Height / 2
        Label17.Left = 300 - lblcenterW
        Label17.Top = 300 - lblcenterH

        Label17.Visible = True

        Return Nothing
    End Function
    Function GameIsOver()
        GameOver = True
        Timer1.Enabled = False
        Timer2.Enabled = False
        WriteText("GAME OVER")
        Return Nothing
    End Function
#End Region




End Class
