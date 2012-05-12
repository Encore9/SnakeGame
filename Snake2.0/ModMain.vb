Module ModMain
#Region "Structure/Variables"

    Structure Parts
        Public x As Long
        Public y As Long
        Public facing As Integer
    End Structure
    Structure foodXY
        Public x As Long
        Public y As Long
    End Structure

    Public snake(99) As Parts
    Public length As Long
    Public food As foodXY
    Public i As Integer = 0

    Public score As Integer
    Public monster As Parts
    Public iteration As Integer = 0
    Public isgoing As Boolean = False
    Public currentIT As Integer


    Public SnakeQuad As Integer
    Public MobQuad As Integer
    Public SnakeSubQuad As Integer
    Public MobSubQuad As Integer
    Public designermode As Boolean = False
    Public GameOver As Boolean = False
    Public changepic As Boolean = True
    Public music As Boolean = False
    Public powerup As Integer
    Public snakespeed As Integer = 90
    Public monsterspeed As Integer = 90
    Public happyend As Boolean = False


#End Region

End Module
