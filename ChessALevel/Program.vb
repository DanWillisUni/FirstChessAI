Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization
Module Module1
    Dim Xaxis(8) As String
    Dim Yaxis(8) As String
    Dim Piece(32) As String
    Dim Location(32) As String
    Dim NumOfMoves(32) As Integer
    Dim Board(8, 8) As Char
    Dim Combinations As Integer = 0
    Const Depth = 0
    Dim Valid As Boolean = True
    Dim GameEnded As Boolean = False
    Dim CurrentGameFile As String
    Dim PieceSymbol(5) As Char
    Dim TotalMoves As Integer
    Dim MainFilePath As String = "C:\Users\Dan\Documents\2018\ComputerScience\"
    Dim Pawn1Coefficent As Integer
    Dim Rook1Coefficent As Integer
    Dim Knight1Coefficent As Integer
    Dim Bishop1Coefficent As Integer
    Dim Queen1Coefficent As Integer
    Dim Move1NumberCoefficent As Double
    Dim ProtectedPieces1Coefficent As Double
    Dim ThreatenedPieces1Coefficent As Double
    Dim Pawn2Coefficent As Integer
    Dim Rook2Coefficent As Integer
    Dim Knight2Coefficent As Integer
    Dim Bishop2Coefficent As Integer
    Dim Queen2Coefficent As Integer
    Dim Move2NumberCoefficent As Double
    Dim ProtectedPieces2Coefficent As Double
    Dim ThreatenedPieces2Coefficent As Double
    Dim KingCoefficent As Integer = 100000
    Dim LastMoveFrom As String = ""
    Dim LastMoveTo As String = ""
    Dim LastMovePiece As Char = ""
    Dim ReviewMoveFrom As String = ""
    Dim ReviewMoveTo As String = ""
    Dim DateNow3 As String = Replace(Convert.ToString(DateTime.Now), "#", "")
    Dim DateNow2() As String = Split(DateNow3, " ")
    Dim DateNow As String = Replace(DateNow2(0), "/", "-")
    Dim TimeNow As String = Replace(DateNow2(1), ":", ";")
    Dim MilliNow As String = DateTime.Now.Millisecond
    Dim LearningMoveFrom As String = ""
    Dim LearningMoveTo As String = ""
    Dim LoadingReason As String = ""
    Dim LearningScore As Integer = 0
    Dim StartDate As New DateTime(1970, 1, 1)
    Dim RegexCoord As New Regex("^[A-H][1|2|3|4|5|6|7|8]$")
    Dim GameMode As String = "NA"
    Dim Player1Strat As Char
    Dim Player2Strat As Char
    Dim CurrentPlayer As Char = "1"
    Dim TriedMoveFrom(0) As String
    Dim TriedMoveTo(0) As String
    Dim OP As Char = "2"
    Const MaxDepth = 1
    Dim InterestingCoefficient As Integer = 800
    Dim Interestings(0) As Integer
    Dim HumPlayer As Char = "1"
    Dim CompPlayer As Char = "2"
    Dim UsingDataBase As Boolean = False
    Dim WritingDataBase As Boolean = True
    Dim CurrentMoveDepth As Integer
    Dim BlackColour As Integer = 13
    Dim WhiteColour As Integer = 12
    Dim SideColour As Integer = 14
    Dim NearEndGame As Boolean = False
    Sub Main()
        Do
            Console.ForegroundColor = ConsoleColor.White
            SetUp()
            Dim TimesGoneThrough As Integer
            TimesGoneThrough += 1
            If TimesGoneThrough = 1 Then
                'Creating all the files
                'Validating FilePath
                Dim mfpv As Boolean = False
                Do
                    Console.WriteLine("What is the main file path?")
                    Console.WriteLine("Example: C:\Users\Dan\Documents\2018\ComputerScience")
                    MainFilePath = Console.ReadLine()
                    Dim MFPR As New Regex("^([A-Z]:)\\([\x20-\x5B\x5D-\x7E]+\\)*([\x20-\x5B\x5D-\x7E]+)+$")
                    If MFPR.Match(MainFilePath).Success = True Then
                        mfpv = True
                    End If
                    If Not System.IO.Directory.Exists(MainFilePath) = True Then
                        mfpv = False
                    End If
                Loop Until mfpv = True
                MainFilePath += "\"
                'MainFilePath = "C:\Users\Dan\Documents\2018\ComputerScience\"
                If Not System.IO.Directory.Exists(MainFilePath + "Chess") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\CVH") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVH")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\CVH\Games") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVH\Games")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\HVH") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\HVH")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\HVH\Games") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\HVH\Games")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\Computer") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\Computer")
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\New.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\New.txt").Dispose()
                    NewBoard()
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\BlackOpenings.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\BlackOpenings.txt").Dispose()
                    If System.IO.File.Exists(MainFilePath + "Chess\Computer\WhiteOpenings.txt") = True Then
                        Openings()
                    End If
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\WhiteOpenings.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\WhiteOpenings.txt").Dispose()
                    Openings()
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\Computer\Learning") Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\Computer\Learning")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\Computer\Learning\TestData") Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\Computer\Learning\TestData")
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\Learning\Results.csv") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\Learning\Results.csv").Dispose()
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\CVC") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVC")
                End If
                If Not System.IO.Directory.Exists(MainFilePath + "Chess\CVC\Games") = True Then
                    System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVC\Games")
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\CVC\All.csv") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\CVC\All.csv").Dispose()
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\CVC\Top.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\CVC\Top.txt").Dispose()
                    Dim objWriter As New System.IO.StreamWriter(MainFilePath + "Chess\CVC\Top.txt")
                    objWriter.WriteLine("100,500,300,300,900,1.0,0.1,0.1")
                    objWriter.Close()
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\CVC\Iterations.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\CVC\Iterations.txt").Dispose()
                    Dim objWriter As New System.IO.StreamWriter(MainFilePath + "Chess\CVC\Iterations.txt")
                    objWriter.WriteLine("0")
                    objWriter.Close()
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\WhiteDataBase.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\WhiteDataBase.txt").Dispose()
                    Dim objWriter As New System.IO.StreamWriter(MainFilePath + "Chess\Computer\WhiteDataBase.txt")
                    Load(MainFilePath + "Chess\Computer\New.txt")
                    objWriter.WriteLine(BoardLine(Board) + ",1:E2:E4")
                    objWriter.Close()
                End If
                If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\BlackDataBase.txt") = True Then
                    System.IO.File.Create(MainFilePath + "Chess\Computer\BlackDataBase.txt").Dispose()
                    Dim objWriter As New System.IO.StreamWriter(MainFilePath + "Chess\Computer\BlackDataBase.txt")
                    Load(MainFilePath + "Chess\Computer\New.txt")
                    Move("1", "E2", "E4")
                    objWriter.WriteLine(BoardLine(Board) + ",1:E7:E5")
                    objWriter.Close()
                End If
                SetUpLearning()
                Intro()
            End If
            'customising colours
            'WhiteColour = ColourChange("white")
            'BlackColour = ColourChange("black")
            'SideColour = ColourChange("the side")
            'menu, selecting gamemode or learning
            Dim Entry As String = "UpArrow"
            Dim k As ConsoleKeyInfo
            Dim Selected As Integer = 2
            Do
                Select Case (Entry)
                    Case "UpArrow"
                        If Not Selected = 1 Then
                            Selected = Selected - 1
                        End If
                    Case "DownArrow"
                        If Not Selected = 4 Then
                            Selected = Selected + 1
                        End If
                End Select
                Do
                    Console.Clear()
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("What gamemode do you want to play?")
                    If Selected = 1 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("CVH: Computer VS Human")
                    If Selected = 2 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("CVC: Computer VS Computer")
                    If Selected = 3 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("HVH: Human VS Human")
                    If Selected = 4 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("Learn")
                    k = Console.ReadKey(True)
                    Entry = k.Key.ToString
                Loop Until Entry = "DownArrow" Or Entry = "UpArrow" Or Entry = "Enter"
            Loop Until Entry = "Enter"
            'deciding which sub to go to
            Select Case (Selected)
                Case 1
                    CVH()
                Case 2
                    CVC()
                Case 3
                    HVH()
                Case 4
                    Learn()
            End Select
        Loop
    End Sub
    'CVC gamemode code
    Sub CVC()
        Player1Strat = "C"
        Player2Strat = "C"
        GameMode = "CVC"
        'play or review menu
        Dim Entry As String = "UpArrow"
        Dim k As ConsoleKeyInfo
        Dim Selected As Integer = 2
        Do
            Select Case (Entry)
                Case "UpArrow"
                    If Not Selected = 1 Then
                        Selected = Selected - 1
                    End If
                Case "DownArrow"
                    If Not Selected = 2 Then
                        Selected = Selected + 1
                    End If
            End Select
            Do
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("What gamemode do you want to play?")
                If Selected = 1 Then
                    Console.ForegroundColor = ConsoleColor.Magenta
                Else
                    Console.ForegroundColor = ConsoleColor.White
                End If
                Console.WriteLine("Play")
                If Selected = 2 Then
                    Console.ForegroundColor = ConsoleColor.Magenta
                Else
                    Console.ForegroundColor = ConsoleColor.White
                End If
                Console.WriteLine("Review")
                k = Console.ReadKey(True)
                Entry = k.Key.ToString
            Loop Until Entry = "DownArrow" Or Entry = "UpArrow" Or Entry = "Enter"
        Loop Until Entry = "Enter"
        Select Case (Selected)
            Case 1
                CVCPlaying()
            Case 2
                Dim GameFile As String
                Do
                    Console.WriteLine("What number game file would you like to review")
                    GameFile = Console.ReadLine()
                Loop Until IsNumeric(GameFile) = True
                Review(GameFile)
        End Select
    End Sub
    Sub CVCPlaying()
        Dim Now As Integer
        'setting stoptime
        Dim StopDate As String = "08/10/2018"
        Dim StopTime As String = "08:00:00"
        'Console.WriteLine("What date would you like to stop")
        'Console.WriteLine("Format: DD/MM/YYYY")
        'StopDate = Console.ReadLine()
        'Console.WriteLine("What time would you like to stop")
        'Console.WriteLine("Format: HH:MM:SS")
        'StopTime = Console.ReadLine()
        Dim StopDateArr() As String = StopDate.Split("/")
        Dim StopTimeArr() As String = StopTime.Split(":")
        Dim StopDateTime As DateTime = New DateTime(Convert.ToInt16(StopDateArr(2)), Convert.ToInt16(StopDateArr(1)), Convert.ToInt16(StopDateArr(0)), Convert.ToInt16(StopTimeArr(0)), Convert.ToInt16(StopTimeArr(1)), Convert.ToInt16(StopTimeArr(2)))
        Dim StopDateTimeInt As Integer = (StopDateTime - StartDate).TotalSeconds
        'making sure that the game number is different
        Dim Iterations As Integer = Convert.ToInt16(ReadLineWithNumberFrom(MainFilePath + "Chess\CVC\Iterations.txt", 1))
        CurrentGameFile = Convert.ToString(Iterations + 1)
        'the whole game code
        Do
            'getting a random set of variables
            Randomize()
            Dim TopLine As String = ReadLineWithNumberFrom(MainFilePath + "Chess\CVC\Top.txt", 1)
            Dim TopLineSplit() As String = TopLine.Split(",")
            'deciding who is white
            If Int(2 * Rnd()) = 0 Then
                Pawn1Coefficent = CInt(Math.Floor((40) * Rnd())) + 80
                Rook1Coefficent = CInt(Math.Floor((200) * Rnd())) + 400
                Knight1Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Bishop1Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Queen1Coefficent = CInt(Math.Floor((360) * Rnd())) + 720
                Move1NumberCoefficent = (CInt(Math.Floor((40) * Rnd())) + 80) / 100
                ProtectedPieces1Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
                ThreatenedPieces1Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
                Pawn2Coefficent = TopLineSplit(0)
                Rook2Coefficent = TopLineSplit(1)
                Knight2Coefficent = TopLineSplit(2)
                Bishop2Coefficent = TopLineSplit(3)
                Queen2Coefficent = TopLineSplit(4)
                Move2NumberCoefficent = TopLineSplit(5)
                ProtectedPieces2Coefficent = TopLineSplit(6)
                ThreatenedPieces2Coefficent = TopLineSplit(7)
            Else
                Pawn2Coefficent = CInt(Math.Floor((40) * Rnd())) + 80
                Rook2Coefficent = CInt(Math.Floor((200) * Rnd())) + 400
                Knight2Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Bishop2Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Queen2Coefficent = CInt(Math.Floor((360) * Rnd())) + 720
                Move2NumberCoefficent = (CInt(Math.Floor((40) * Rnd())) + 80) / 100
                ProtectedPieces2Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
                ThreatenedPieces2Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
                Pawn1Coefficent = TopLineSplit(0)
                Rook1Coefficent = TopLineSplit(1)
                Knight1Coefficent = TopLineSplit(2)
                Bishop1Coefficent = TopLineSplit(3)
                Queen1Coefficent = TopLineSplit(4)
                Move1NumberCoefficent = TopLineSplit(5)
                ProtectedPieces1Coefficent = TopLineSplit(6)
                ThreatenedPieces1Coefficent = TopLineSplit(7)
            End If
            Now = (DateTime.Now - StartDate).TotalSeconds
            Iterations += 1
            CurrentGameFile = Convert.ToString(Iterations)
            Dim GameName As String = Convert.ToString(Iterations)
            'loading new game
            Load(MainFilePath + "Chess\Computer\New.txt")
            'creating new gamefile
            If Not System.IO.Directory.Exists(MainFilePath + "Chess\CVC\Games\" + GameName) = True Then
                System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVC\Games\" + GameName)
                System.IO.Directory.CreateDirectory(MainFilePath + "Chess\CVC\Games\" + GameName + "\Moves")
                System.IO.File.Create(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\CompressedMove.txt").Dispose()
            End If
            Save(GameName, "CurrentBoard", "Current", False)
            CurrentPlayer = "1"
            Dim FILE_NAME As String = MainFilePath + "Chess\CVC\Iterations.txt"
            Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
            objWriter.WriteLine(Convert.ToString(Iterations))
            objWriter.Close()
            'actually playing the game
            Do
                ComputerMove(False)
                Display("1")
                'saving the game
                Save(CurrentGameFile + "\Moves", Convert.ToString(TotalMoves) + "Computer" + CurrentPlayer, "Move " + LastMoveFrom + ":" + LastMoveTo, False)
                Save(CurrentGameFile, "CurrentBoard", "Current", False)
                'switching players
                If CurrentPlayer = "1" Then
                    CurrentPlayer = "2"
                Else
                    CurrentPlayer = "1"
                End If
                'checking if the game is over    
            Loop Until CheckMated(Board, Location, NumOfMoves, CurrentPlayer, Piece) = True Or IsDrawn() <> "F"
            Dim EndReason As String = IsDrawn()
            'determining what to write to the file
            If EndReason = "F" Then
                'they didnt draw
                'finding out who won
                If CheckMated(Board, Location, NumOfMoves, "2", Piece) = True Then
                    Dim objWriterTOP As New StreamWriter(MainFilePath + "Chess\CVC\Top.txt")
                    objWriterTOP.WriteLine(Convert.ToString(Pawn1Coefficent) + "," + Convert.ToString(Rook1Coefficent) + "," + Convert.ToString(Knight1Coefficent) + "," + Convert.ToString(Bishop1Coefficent) + "," + Convert.ToString(Queen1Coefficent) + "," + Convert.ToString(Move1NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces1Coefficent) + "," + Convert.ToString(ProtectedPieces1Coefficent) + "," + Convert.ToString(ThreatenedPieces1Coefficent) + "," + Convert.ToString(ProtectedPieces1Coefficent))
                    objWriterTOP.Close()
                    Using writer As New StreamWriter(MainFilePath + "Chess\CVC\All.csv", True)
                        writer.WriteLine(Convert.ToString(Iterations) + "," + Convert.ToString(Pawn1Coefficent) + "," + Convert.ToString(Rook1Coefficent) + "," + Convert.ToString(Knight1Coefficent) + "," + Convert.ToString(Bishop1Coefficent) + "," + Convert.ToString(Queen1Coefficent) + "," + Convert.ToString(Move1NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces1Coefficent) + "," + Convert.ToString(ProtectedPieces1Coefficent) + ",Beat," + Convert.ToString(Pawn2Coefficent) + "," + Convert.ToString(Rook2Coefficent) + "," + Convert.ToString(Knight2Coefficent) + "," + Convert.ToString(Bishop2Coefficent) + "," + Convert.ToString(Queen2Coefficent) + "," + Convert.ToString(Move2NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces2Coefficent) + "," + Convert.ToString(ProtectedPieces2Coefficent))
                    End Using
                    'writing to the database
                    DataBaseResults("1")
                Else
                    Using writer As New StreamWriter(MainFilePath + "Chess\CVC\All.csv", True)
                        writer.WriteLine(Convert.ToString(Iterations) + "," + Convert.ToString(Pawn2Coefficent) + "," + Convert.ToString(Rook2Coefficent) + "," + Convert.ToString(Knight2Coefficent) + "," + Convert.ToString(Bishop2Coefficent) + "," + Convert.ToString(Queen2Coefficent) + "," + Convert.ToString(Move2NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces2Coefficent) + "," + Convert.ToString(ProtectedPieces2Coefficent) + ",Beat," + Convert.ToString(Pawn1Coefficent) + "," + Convert.ToString(Rook1Coefficent) + "," + Convert.ToString(Knight1Coefficent) + "," + Convert.ToString(Bishop1Coefficent) + "," + Convert.ToString(Queen1Coefficent) + "," + Convert.ToString(Move1NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces1Coefficent) + "," + Convert.ToString(ProtectedPieces1Coefficent))
                    End Using
                    'writing to the database
                    DataBaseResults("2")
                End If
            Else
                'they drew
                Using writer As New StreamWriter(MainFilePath + "Chess\CVC\All.csv", True)
                    writer.WriteLine(Convert.ToString(Iterations) + "," + Convert.ToString(Pawn1Coefficent) + "," + Convert.ToString(Rook1Coefficent) + "," + Convert.ToString(Knight1Coefficent) + "," + Convert.ToString(Bishop1Coefficent) + "," + Convert.ToString(Queen1Coefficent) + "," + Convert.ToString(Move1NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces1Coefficent) + "," + Convert.ToString(ProtectedPieces1Coefficent) + ",Drew " + EndReason + "," + Convert.ToString(Pawn2Coefficent) + "," + Convert.ToString(Rook2Coefficent) + "," + Convert.ToString(Knight2Coefficent) + "," + Convert.ToString(Bishop2Coefficent) + "," + Convert.ToString(Queen2Coefficent) + "," + Convert.ToString(Move2NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces2Coefficent) + "," + Convert.ToString(ProtectedPieces2Coefficent))
                End Using
            End If
        Loop Until Now > StopDateTimeInt
    End Sub
    'code for CVH
    Sub CVH()
        GameMode = "CVH"
        'getting the top coefficients
        Dim TopLine As String = ReadLineWithNumberFrom(MainFilePath + "Chess\CVC\Top.txt", 1)
        Dim TopLineSplit() As String = TopLine.Split(",")
        LoadorNew()
        If LoadingReason(0) <> "N" Then
            'finding out who the computer was in the loaded game
            If LoadingReason(8) = "1" Then
                HumPlayer = "1"
                CompPlayer = "2"
            Else
                HumPlayer = "2"
                CompPlayer = "1"
            End If
        Else
            Randomize()
            'deciding who is white in the new game
            If Int(Rnd() * 2) = 0 Then
                Pawn2Coefficent = TopLineSplit(0)
                Rook2Coefficent = TopLineSplit(1)
                Knight2Coefficent = TopLineSplit(2)
                Bishop2Coefficent = TopLineSplit(3)
                Queen2Coefficent = TopLineSplit(4)
                Move2NumberCoefficent = TopLineSplit(5)
                ProtectedPieces2Coefficent = TopLineSplit(6)
                ThreatenedPieces2Coefficent = TopLineSplit(7)
                Player1Strat = "H"
                Player2Strat = "C"
            Else
                Pawn1Coefficent = TopLineSplit(0)
                Rook1Coefficent = TopLineSplit(1)
                Knight1Coefficent = TopLineSplit(2)
                Bishop1Coefficent = TopLineSplit(3)
                Queen1Coefficent = TopLineSplit(4)
                Move1NumberCoefficent = TopLineSplit(5)
                ProtectedPieces1Coefficent = TopLineSplit(6)
                ThreatenedPieces1Coefficent = TopLineSplit(7)
                Player2Strat = "H"
                Player1Strat = "C"
            End If
            If Player2Strat = "H" Then
                HumPlayer = "2"
                CompPlayer = "1"
            End If
        End If
        'actual game code
        Do
            If HumPlayer = "1" Then
                HumanFullMove()
                ComputerFullMove()
            Else
                ComputerFullMove()
                HumanFullMove()
            End If
            'checking if the game ended
        Loop Until GameEnded = True Or CheckMated(Board, Location, NumOfMoves, HumPlayer, Piece) = True Or IsDrawn() <> "F"
        If CheckMated(Board, Location, NumOfMoves, HumPlayer, Piece) = True Then
            DataBaseResults(CompPlayer)
        End If
    End Sub
    'all of the part of a move that a human makes
    Sub HumanFullMove()
        Do
            'checking if the human is in check
            If InCheck(HumPlayer, Board, Location, NumOfMoves) = True Then
                Console.WriteLine("You are in Check")
            End If
            Dim User As String
            Do
                'prints it out oriented so that the human playings pieces are on the bottom
                Display(HumPlayer)
                User = Console.ReadLine()
                If User <> "" Then
                    'check of the input was a command
                    Commands(User)
                End If
            Loop Until User = ""
            CurrentPlayer = HumPlayer
            HumMove()
        Loop Until Valid = True
        If HumPlayer = "2" Then
            'save the board
            Save(CurrentGameFile, "CurrentBoard", "Current", False)
        End If
        'display the board again
        Display(HumPlayer)
        'save the move
        Save(CurrentGameFile + "\Moves", Convert.ToString(TotalMoves) + "Human", "Move " + LastMoveFrom + ":" + LastMoveTo, False)
    End Sub
    'everything a computer does on its turn
    Sub ComputerFullMove()
        'displays so that the computer is ontop
        Display(HumPlayer)
        'check if the computer haas lost
        If CheckMated(Board, Location, NumOfMoves, CompPlayer, Piece) = False Then
            'check if the computer is in check
            If InCheck(CompPlayer, Board, Location, NumOfMoves) = True Then
                Console.WriteLine("Computer is in Check")
            End If
            Console.WriteLine("Waiting for computers move")
            CurrentPlayer = CompPlayer
            If CheckMated(Board, Location, NumOfMoves, CurrentPlayer, Piece) = True Or IsDrawn() <> "F" Then
                GameEnded = True
            End If
            'most of the computers move
            ComputerMove(False)
            'saving the current board
            If HumPlayer = "1" Then
                Save(CurrentGameFile, "CurrentBoard", "Current", False)
            End If
            'saving the move
            Save(CurrentGameFile + "\Moves", Convert.ToString(TotalMoves) + "Computer", "Move " + LastMoveFrom + ":" + LastMoveTo, False)
        Else
            'if the human won
            'writing to the database
            DataBaseResults(HumPlayer)
            Console.WriteLine("You won")
            Console.ReadLine()
            GameEnded = True
        End If
    End Sub
    '2 player
    Sub HVH()
        Player1Strat = "H"
        Player2Strat = "H"
        GameMode = "HVH"
        'loading or new game
        LoadorNew()
        CurrentPlayer = "1"
        OP = "2"
        Do
            Do
                Dim User As String
                Do
                    'display for whited turn
                    Display("1")
                    User = Console.ReadLine()
                    If User <> "" Then
                        'check commands
                        Commands(User)
                    End If
                Loop Until User = ""
                Valid = False
                'see if p1 is in check
                If InCheck(CurrentPlayer, Board, Location, NumOfMoves) = True Then
                    Console.WriteLine("Player" + CurrentPlayer + " is in Check")
                End If
                'player1 makes a move
                HumMove()
            Loop Until Valid = True
            'save the move
            Save(CurrentGameFile + "\Moves", Convert.ToString(TotalMoves) + "Human" + CurrentPlayer, "Move " + LastMoveFrom + ":" + LastMoveTo, False)
            'see if p2 is in check
            If InCheck(OP, Board, Location, NumOfMoves) = True Then
                Console.WriteLine("Player" + OP + " is in Check")
            End If
            'see if the game is over
            If CheckMated(Board, Location, NumOfMoves, CurrentPlayer, Piece) = True Or MoveInDraw(5, Board) = True Then
                GameEnded = True
            End If
            Do
                'p2s move
                HumMove()
            Loop Until Valid = True
            'save move and then board
            Save(CurrentGameFile + "\Moves", Convert.ToString(TotalMoves) + "Human" + CurrentPlayer, "Move " + LastMoveFrom + ":" + LastMoveTo, False)
            Save(CurrentGameFile, "CurrentBoard", "Current", False)
        Loop Until GameEnded = True
    End Sub
    'returns True if Player is checkmated and False otherwise
    Function CheckMated(ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumberOfMoves() As Integer, ByVal Player As Char, ByVal TempPieces() As String) As Boolean
        Dim CheckMate As Boolean = False
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        Dim CheckTimes As Integer = 0
        'gets a array of possible moves
        Possible(Player, TempBoard, TempLocation, TempNumberOfMoves, MoveFrom, MoveTo, True)
        'goes through the array
        For i = 1 To MoveFrom.Length - 1
            'copys the arrays to come back to later
            Dim BoardAfterMove(8, 8) As Char
            CopyBoards(TempBoard, BoardAfterMove)
            Dim LocationAfterMove(0) As String
            CopyStrArr(TempLocation, LocationAfterMove)
            Dim NumOfMoveAfterMove(0) As Integer
            CopyIntArr(TempNumberOfMoves, NumOfMoveAfterMove)
            'simulated the moves on the copys
            SimMove(BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove, MoveFrom(i), MoveTo(i), TempPieces)
            'sees if the player is in check
            If InCheck(Player, BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove) = True Then
                CheckTimes += 1
            End If
        Next
        'if the player is in check for all the possible moves
        If CheckTimes = MoveFrom.Length - 1 Then
            CheckMate = True
        End If
        Return CheckMate
    End Function
    Function IsDrawn()
        Dim IsaDraw As String = "F"
        'checks if it is fivefold draw
        If MoveInDraw(5, Board) = True Then
            IsaDraw = "FF"
        End If
        'if it isnt fivefold draw
        If IsaDraw = "F" Then
            'gets a array of the possible moves
            Dim PossMoveFrom(0) As String
            Dim PossMoveTo(0) As String
            Possible(CurrentPlayer, Board, Location, NumOfMoves, PossMoveFrom, PossMoveTo, True)
            'sees if there is any moves that Player can make
            If PossMoveFrom.Length = 0 And InCheck(CurrentPlayer, Board, Location, NumOfMoves) = False Then
                'declared stalemate as there are no legal moves and the player is not in check
                IsaDraw = "SM"
            End If
            If IsaDraw = "F" Then
                'calculated if there is insufficient material or not
                Dim PawnNumber As Integer
                Dim RookNumber As Integer
                Dim KnightNumber As Integer
                Dim BishopNumber As Integer
                Dim QueenNumber As Integer
                Dim OPPawnNumber As Integer
                Dim OPRookNumber As Integer
                Dim OPKnightNumber As Integer
                Dim OPBishopNumber As Integer
                Dim OPQueenNumber As Integer
                'goes through every square on the board to count the pieces
                For y = 1 To 8
                    For x = 1 To 8
                        If Board(y, x) <> " " Then
                            'adds 1 to the correct number depending on what is in the square
                            Select Case (Board(y, x))
                                Case "p"
                                    OPPawnNumber += 1
                                Case "r"
                                    OPRookNumber += 1
                                Case "n"
                                    OPKnightNumber += 1
                                Case "b"
                                    OPBishopNumber += 1
                                Case "q"
                                    OPQueenNumber += 1
                                Case "P"
                                    PawnNumber += 1
                                Case "R"
                                    RookNumber += 1
                                Case "N"
                                    KnightNumber += 1
                                Case "B"
                                    BishopNumber += 1
                                Case "Q"
                                    QueenNumber += 1
                            End Select
                        End If
                    Next
                Next
                'if there is no pawwns left because of promotion
                If (PawnNumber + OPPawnNumber) = 0 Then
                    'if there is no pieces of the opponents other than their king
                    If OPRookNumber + OPKnightNumber + OPBishopNumber + OPQueenNumber = 0 Then
                        'checking to see if the current player has enough material to mate
                        Select Case (RookNumber + KnightNumber + BishopNumber + QueenNumber)
                            Case 0
                                IsaDraw = "IM"
                            Case 1
                                If RookNumber = 1 Or KnightNumber = 1 Then
                                    IsaDraw = "IM"
                                End If
                            Case 2
                                If KnightNumber = 2 Then
                                    IsaDraw = "IM"
                                End If
                        End Select
                    End If
                    'insuffcient material is declared if currentplayer doesnt have enough material
                End If
            End If
        End If
        Return IsaDraw
    End Function
    'counting if the current boardstate has happened over "a" times
    Function MoveInDraw(ByVal a As Integer, ByVal TheBoard(,) As Char) As Boolean
        Dim Drawing As Boolean = False
        Dim Times As Integer = 0
        'goes through the compressedmove file line by line
        For i = 1 To File.ReadAllLines(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\CompressedMove.txt").Length
            'if it finds the board state in there it adds one to the counter
            If BoardLine(TheBoard) = ReadLineWithNumberFrom(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\CompressedMove.txt", i) Then
                Times += 1
            End If
        Next
        'if the counter is bigger than "a" then it returns True
        If Times >= a Then
            Drawing = True
        End If
        Return Drawing
    End Function
    'converts the boardstate array into a 64char string
    Function BoardLine(ByVal TempBoard(,) As Char) As String
        Dim BLine As String = ""
        'top left to bottom right
        For y = 1 To 8
            For x = 1 To 8
                BLine = BLine + (TempBoard(x, y))
            Next
        Next
        Return BLine
    End Function
    'gives a timestamp of the time now
    Function TimestampNow() As String
        Dim DateNow3 As String = Replace(Convert.ToString(DateTime.Now), "#", "")
        Dim DateNow2() As String = Split(DateNow3, " ")
        Dim MilliNow As String = DateTime.Now.Millisecond
        Return DateNow2(1) + ":" + MilliNow
    End Function
    'fills the openings file for black and white
    Sub Openings()
        Dim Possibles(9) As String
        'E2:E4
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Possibles(0) = "E2:E4"
        Possibles(1) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "E2", "E4")
        Possibles(0) = "E7:E5"
        Possibles(1) = "C7:C5"
        Possibles(2) = "B7:B6"
        Possibles(3) = "B8:C6"
        Possibles(4) = "G8:F6"
        Possibles(5) = "G7:G6"
        Possibles(6) = "D7:D5"
        Possibles(7) = "D7:D6"
        Possibles(8) = "C7:C6"
        Possibles(9) = "E7:E6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 9, "BlackOpenings")
        'E7:E5
        Move("2", "E7", "E5")
        Possibles(0) = "G1:F3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "G1", "F3")
        Possibles(0) = "G8:F6"
        Possibles(1) = "B8:C6"
        Possibles(2) = "F7:F5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 2, "BlackOpenings")
        Move("2", "B8", "C6")
        Possibles(0) = "F1:B5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "F1", "B5")
        Possibles(0) = "A7:A6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "A7", "A6")
        Possibles(0) = "B5:C6"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B5", "C6")
        Possibles(0) = "D7:C6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "D7", "C6")
        Possibles(0) = "F3:E5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "F3", "E5")
        Possibles(0) = "D8:D4"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        'C7:C5
        Move("1", "E2", "E4")
        Move("2", "C7", "C5")
        Possibles(0) = "G1:F3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "G1", "F3")
        Possibles(0) = "D7:D6"
        Possibles(1) = "E7:E6"
        Possibles(2) = "G7:G6"
        Possibles(3) = "B8:C6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 3, "BlackOpenings")
        Move("2", "D7", "D6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "C5:D4"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "C5", "D4")
        Possibles(0) = "F3:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "F3", "D4")
        Possibles(0) = "G8:F6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "G8", "F6")
        Possibles(0) = "B1:C3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "A7:A6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "A7", "A6")
        Possibles(0) = "C1:G5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "C1", "G5")
        Possibles(0) = "E7:E6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'B7:B6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "B7", "B6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "C8:B7"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "C8", "B7")
        Possibles(0) = "B1:C3"
        Possibles(1) = "F1:D1"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 1, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "E7:E6"
        Possibles(1) = "G8:F6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "B7", "B6")
        Move("1", "D2", "D4")
        Move("2", "C8", "B7")
        Move("1", "F1", "D3")
        Possibles(0) = "E7:E6"
        Possibles(1) = "F7:F5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        'B8:C6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "B8", "C6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "D7:D5"
        Possibles(1) = "E7:E5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        Move("2", "D7", "D5")
        Possibles(0) = "E4:E5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "E4", "E5")
        Possibles(0) = "C8:F5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "B8", "C6")
        Move("1", "D2", "D4")
        Move("2", "E7", "E5")
        Possibles(0) = "D4:D5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D4", "D5")
        Possibles(0) = "C6:E8"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'G8:F6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "G8", "F6")
        Possibles(0) = "E4:E5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "E4", "E5")
        Possibles(0) = "F6:D5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "F6", "D5")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "D7:D6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'G7:G6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "G7", "G6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "F8:G7"
        Possibles(1) = "B8:C6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        Move("2", "F8", "G7")
        Move("1", "B1", "C3")
        Possibles(0) = "D7:D6"
        Possibles(1) = "G8:F6"
        Possibles(2) = "B8:C6"
        Possibles(3) = "A7:A6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 3, "BlackOpenings")
        'D7:D5
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "D7", "D5")
        Possibles(0) = "E4:E5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "E4", "D5")
        Possibles(0) = "D8:D5"
        Possibles(1) = "G8:F6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        Move("2", "D8", "D5")
        Possibles(0) = "B1:C3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "D5:D6"
        Possibles(1) = "D5:D8"
        Possibles(2) = "D5:E5"
        Possibles(3) = "D5:A5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 3, "BlackOpenings")
        Move("2", "D5", "A5")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "G8:F6"
        Possibles(1) = "C7:C6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        'D7:D6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "D7", "D6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "G8:F6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "G8", "F6")
        Possibles(0) = "B1:C3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "G7:G6"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        'C7:C6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "C7", "C6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "G8:F6"
        Possibles(1) = "D7:D5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        Move("2", "D7", "D5")
        Possibles(0) = "B1:C3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "D5:E4"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "D5", "E4")
        Possibles(0) = "C3:E4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "C3", "E4")
        Possibles(0) = "C8:F5"
        Possibles(1) = "G8:F6"
        Possibles(2) = "B8:D7"
        'write to BlackOpenings
        SaveBoardState(Possibles, 2, "BlackOpenings")
        'E7:E6
        'loading new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        Move("1", "E2", "E4")
        Move("2", "E7", "E6")
        Possibles(0) = "D2:D4"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "D2", "D4")
        Possibles(0) = "D7:D5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "D7", "D5")
        Possibles(0) = "B1:C3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "B1", "C3")
        Possibles(0) = "G8:F6"
        Possibles(1) = "F8:B4"
        'write to BlackOpenings
        SaveBoardState(Possibles, 1, "BlackOpenings")
        Move("2", "F8", "B4")
        Possibles(0) = "E4:E5"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "E4", "E5")
        Possibles(0) = "C7:C5"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
        Move("2", "C7", "C5")
        Possibles(0) = "A2:A3"
        'write to WhiteOpenings
        SaveBoardState(Possibles, 0, "WhiteOpenings")
        Move("1", "A2", "A3")
        Possibles(0) = "B4:C3"
        'write to BlackOpenings
        SaveBoardState(Possibles, 0, "BlackOpenings")
    End Sub
    'saving for the opening moves
    Sub SaveBoardState(ByVal Possibles() As String, ByVal Num As Integer, ByVal FileName As String)
        Dim FILE_NAME As String = MainFilePath + "Chess\Computer\" + FileName + ".txt"
        'created the file if it doesnt exist
        If Not System.IO.File.Exists(FILE_NAME) = True Then
            System.IO.File.Create(FILE_NAME).Dispose()
        End If
        Dim InputString As String = BoardLine(Board)
        For i = 0 To Num
            InputString += (",1:" + Possibles(i))
        Next
        'writes to the file keeping what is already in the file
        Using writer As New StreamWriter(FILE_NAME, True)
            writer.WriteLine(InputString)
        End Using
    End Sub
    'writes to create a new.txt
    Sub NewBoard()
        Dim InputString(43) As String
        InputString(0) = "RNBQKBNR|8"
        InputString(1) = "PPPPPPPP|7"
        InputString(2) = "        |6"
        InputString(3) = "        |5"
        InputString(4) = "        |4"
        InputString(5) = "        |3"
        InputString(6) = "pppppppp|2"
        InputString(7) = "rnbqkbnr|1"
        InputString(8) = "--------+"
        InputString(9) = "ABCDEFGH"
        InputString(10) = "HP1p: A2: 0"
        InputString(11) = "HP2p: B2: 0"
        InputString(12) = "HP3p: C2: 0"
        InputString(13) = "HP4p: D2: 0"
        InputString(14) = "HP5p: E2: 0"
        InputString(15) = "HP6p: F2: 0"
        InputString(16) = "HP7p: G2: 0"
        InputString(17) = "HP8p: H2: 0"
        InputString(18) = "HR1r: A1: 0"
        InputString(19) = "HN1n: B1: 0"
        InputString(20) = "HB1b: C1: 0"
        InputString(21) = "HQ1q: D1: 0"
        InputString(22) = "HK1k: E1: 0"
        InputString(23) = "HB2b: F1: 0"
        InputString(24) = "HN2n: G1: 0"
        InputString(25) = "HR2r: H1: 0"
        InputString(26) = "CP1p: A7: 0"
        InputString(27) = "CP2p: B7: 0"
        InputString(28) = "CP3p: C7: 0"
        InputString(29) = "CP4p: D7: 0"
        InputString(30) = "CP5p: E7: 0"
        InputString(31) = "CP6p: F7: 0"
        InputString(32) = "CP7p: G7: 0"
        InputString(33) = "CP8p: H7: 0"
        InputString(34) = "CR1r: A8: 0"
        InputString(35) = "CN1n: B8: 0"
        InputString(36) = "CB1b: C8: 0"
        InputString(37) = "CQ1q: D8: 0"
        InputString(38) = "CK1k: E8: 0"
        InputString(39) = "CB2b: F8: 0"
        InputString(40) = "CN2n: G8: 0"
        InputString(41) = "CR2r: H8: 0"
        InputString(42) = "0"
        InputString(43) = "New"
        'goes throigh the array and writting it to the file line by line
        For i = 0 To 43
            Using writer As New StreamWriter(MainFilePath + "Chess\Computer\New.txt", True)
                writer.WriteLine(InputString(i))
            End Using
        Next
    End Sub
    'sets up the learning testdata for my first learning style
    Sub SetUpLearning()
        'goes through creating all the text files for them if they dont exist
        For i = 1 To 16
            If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\Learning\TestData\" + Convert.ToString(i) + ".txt") = True Then
                System.IO.File.Create(MainFilePath + "Chess\Computer\Learning\TestData\" + Convert.ToString(i) + ".txt").Dispose()
            End If
        Next
        'Magnus Carlsen Crushes GM Yu Yangyi (2736) In 16 Moves | Blitz Chess Playoff Qatar Chess Master 2015
        'load new game
        Load(MainFilePath + "Chess\Computer\New.txt")
        'make the move white made
        Move("1", "D2", "D4")
        'save the board and the movemade by Magnus
        Save("", "1", "G8:F6", True)
        'make the move thye both made
        Move("2", "G8", "F6")
        Move("1", "C2", "C4")
        'save the board and the movemade by Magnus
        Save("", "2", "E7:E6", True)
        'make the move thye both made
        Move("2", "E7", "E6")
        Move("1", "B1", "C3")
        'save the board and the movemade by Magnus
        Save("", "3", "F8:B4", True)
        'make the move thye both made
        Move("2", "F8", "B4")
        Move("1", "G1", "F3")
        'save the board and the movemade by Magnus
        Save("", "4", "B7:B6", True)
        'make the move thye both made
        Move("2", "B7", "B6")
        Move("1", "E2", "E3")
        'save the board and the movemade by Magnus
        Save("", "5", "C8:B7", True)
        'make the move thye both made
        Move("2", "C8", "B7")
        Move("1", "F1", "D3")
        'save the board and the movemade by Magnus
        Save("", "6", "E8:G8", True)
        'make the move thye both made
        Move("2", "E8", "G8")
        Move("1", "E1", "G1")
        'save the board and the movemade by Magnus
        Save("", "7", "C7:C5", True)
        'make the move thye both made
        Move("2", "C7", "C5")
        Move("1", "C3", "A4")
        'save the board and the movemade by Magnus
        Save("", "8", "C5:D4", True)
        'make the move thye both made
        Move("2", "C5", "D4")
        Move("1", "E3", "D4")
        'save the board and the movemade by Magnus
        Save("", "9", "F8:E8", True)
        'make the move thye both made
        Move("2", "F8", "E8")
        Move("1", "A2", "A3")
        'save the board and the movemade by Magnus
        Save("", "10", "C5:F8", True)
        'make the move thye both made
        Move("2", "C5", "F8")
        Move("1", "C1", "F4")
        'save the board and the movemade by Magnus
        Save("", "11", "B7:F3", True)
        'make the move thye both made
        Move("2", "B7", "F3")
        Move("1", "D1", "F3")
        'save the board and the movemade by Magnus
        Save("", "12", "B8:C6", True)
        'make the move thye both made
        Move("2", "B8", "C6")
        Move("1", "D4", "D5")
        'save the board and the movemade by Magnus
        Save("", "13", "E6:D5", True)
        'make the move thye both made
        Move("2", "E6", "D5")
        Move("1", "C4", "D5")
        'save the board and the movemade by Magnus
        Save("", "14", "C6:F5", True)
        'make the move thye both made
        Move("2", "C6", "F5")
        Move("1", "F3", "D1")
        'save the board and the movemade by Magnus
        Save("", "15", "F5:D3", True)
        'make the move thye both made
        Move("2", "F5", "D3")
        Move("1", "D1", "D3")
        'save the board and the movemade by Magnus
        Save("", "16", "E8:E4", True)
    End Sub
    'does the commands that a used enters
    Sub Commands(ByVal User As String)
        Select Case (LCase(User))
            Case "exit"
                End
            Case "load"
                Main()
            Case "review"
                Review(CurrentGameFile)
            Case Else
                'error message
                Console.WriteLine("Un-recognised command")
                Console.WriteLine("Exit. Exit")
                Console.WriteLine("Load. Load Or New")
                Console.WriteLine("Review. Review the loaded game")
                Console.ReadLine()
        End Select
    End Sub
    'displaying the board in review mode
    Sub DisplayReview()
        'getting the coords of the peice moves that turn to highlight it
        Dim MoveToX As Integer = ToCoordX(ReviewMoveTo(0))
        Dim MoveToY As Integer = ToCoordY(ReviewMoveTo(1))
        If ReviewMoveTo = "NA" Then
            MoveToX = 0
            MoveToY = 0
        Else
            MoveToX = ToCoordX(ReviewMoveTo(0))
            MoveToY = ToCoordY(ReviewMoveTo(1))
        End If
        Dim MoveFromX As Integer = ToCoordX(ReviewMoveFrom(0))
        Dim MoveFromY As Integer = ToCoordY(ReviewMoveFrom(1))
        If ReviewMoveFrom = "NA" Then
            MoveFromX = 0
            MoveFromY = 0
        Else
            MoveFromX = ToCoordX(ReviewMoveFrom(0))
            MoveFromY = ToCoordY(ReviewMoveFrom(1))
        End If
        'refresh the board so everything is in the correct place
        RefreshBoard()
        Console.Clear()
        Dim BackColour As Integer = 0
        'display
        For y = 1 To 8
            Console.BackgroundColor = ConsoleColor.Black
            Console.Write("   ")
            'black or white square
            If IsOdd(y) = True Then
                BackColour = 0
            Else
                BackColour = 15
            End If
            'black or whit square
            For x = 1 To 8
                If BackColour = 0 Then
                    BackColour = 15
                Else
                    BackColour = 0
                End If
                'checking if it matches the coords of the peice just moved
                If (y = MoveFromY And x = MoveFromX) Or (y = MoveToY And x = MoveToX) Then
                    Console.BackgroundColor = ConsoleColor.Green
                Else
                    Console.BackgroundColor = BackColour
                End If
                Console.Write("       ")
            Next
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            'black or white square
            If IsOdd(y) = True Then
                BackColour = 0
            Else
                BackColour = 15
            End If
            For x = 1 To 8
                If BackColour = 0 Then
                    BackColour = 15
                Else
                    BackColour = 0
                End If
                'Axis labels
                If x = 1 Then
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.Cyan
                    Console.Write(" " + Yaxis(y) + " ")
                End If
                'making the colours correct
                Dim ForColour As Integer = ForeColour(x, y)
                Console.ForegroundColor = ForColour
                'highlighting green for the move that just happened
                If (y = MoveFromY And x = MoveFromX) Or (y = MoveToY And x = MoveToX) Then
                    Console.BackgroundColor = ConsoleColor.Green
                Else
                    Console.BackgroundColor = BackColour
                End If
                Console.Write("   " + UCase(Board(x, y)) + "   ")
            Next
            'black or white square
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            Console.Write("   ")
            If IsOdd(y) = True Then
                BackColour = 0
            Else
                BackColour = 15
            End If
            'black or white square
            For x = 1 To 8
                If BackColour = 0 Then
                    BackColour = 15
                Else
                    BackColour = 0
                End If
                'highlight green
                If (y = MoveFromY And x = MoveFromX) Or (y = MoveToY And x = MoveToX) Then
                    Console.BackgroundColor = ConsoleColor.Green
                Else
                    Console.BackgroundColor = BackColour
                End If
                Console.Write("       ")
            Next
            'set it all back to how it was
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
        Next
    End Sub
    'loads or creates a game
    Sub LoadorNew()
        NearEndGame = False
        Dim GameName As String = ""
        Dim Again As Boolean = True
        'menu
        Do
            Console.Clear()
            Dim Entry As String = "UpArrow"
            Dim k As ConsoleKeyInfo
            Dim Selected As Integer = 2
            Do
                Select Case (Entry)
                    Case "UpArrow"
                        If Not Selected = 1 Then
                            Selected = Selected - 1
                            '-Debug("LoadOrNew/Down")
                        End If
                    Case "DownArrow"
                        If Not Selected = 2 Then
                            Selected = Selected + 1
                            '-Debug("LoadOrNew/Up")
                        End If
                End Select
                Do
                    Console.Clear()
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("Do you want to")
                    If Selected = 1 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("Load a existing game")
                    If Selected = 2 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Console.WriteLine("Create a new game")
                    k = Console.ReadKey(True)
                    Entry = k.Key.ToString
                Loop Until Entry = "DownArrow" Or Entry = "UpArrow" Or Entry = "Enter"
            Loop Until Entry = "Enter"
            Select Case (Selected)
                Case 1 ' load
                    Console.ForegroundColor = ConsoleColor.White
                    'userinput
                    Console.WriteLine("What is the name of the game you wish to load?")
                    GameName = Console.ReadLine()
                    'try to find the game
                    If System.IO.File.Exists(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\CurrentBoard.txt") = True Then
                        Load(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\CurrentBoard.txt")
                        Again = False
                    Else
                        'no game with that name was found
                        Console.WriteLine("Sorry that is not a game")
                    End If
                Case 2
                    Console.ForegroundColor = ConsoleColor.White
                    'userinput of the new game name
                    Console.WriteLine("What do you want to call the game")
                    GameName = Console.ReadLine()
                    'loads new game
                    Load(MainFilePath + "Chess\Computer\New.txt")
                    'if the game doesnt alread yexist create it
                    If Not System.IO.Directory.Exists(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName) = True Then
                        Again = False
                        'create everything to do with the game file
                        System.IO.Directory.CreateDirectory(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName)
                        System.IO.Directory.CreateDirectory(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\Moves")
                        System.IO.File.Create(MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\CompressedMove.txt").Dispose()
                        'save the game
                        Save(GameName, "CurrentBoard", "Current", False)
                    Else
                        'there is already a game with that name
                        Console.WriteLine("A game called this already exists")
                    End If
            End Select
        Loop Until Again = False
        CurrentGameFile = GameName
    End Sub
    'saves the game
    Sub Save(ByVal GameName As String, ByVal FileName As String, ByVal Reason As String, ByVal Learning As Boolean)
        Dim FILE_NAME As String
        'determining where to save the game because this is used to generate the test data
        If Learning = False Then
            FILE_NAME = MainFilePath + "Chess\" + GameMode + "\Games\" + GameName + "\" + FileName + ".txt"
        Else
            FILE_NAME = MainFilePath + "Chess\Computer\Learning\TestData\" + FileName + ".txt"
        End If
        'if the file doesnt exist create it
        If Not System.IO.File.Exists(FILE_NAME) = True Then
            System.IO.File.Create(FILE_NAME).Dispose()
        End If
        'write the board to the file
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        For y = 1 To 8
            For x = 1 To 8
                If x = 8 Then
                    objWriter.WriteLine(Board(x, y) + "|" + Convert.ToString(9 - y))
                Else
                    objWriter.Write(Board(x, y))
                End If
            Next
        Next
        objWriter.WriteLine("--------+")
        For i = 1 To 8
            objWriter.Write(UCase(Xaxis(i)))
        Next
        objWriter.WriteLine()
        'write the pieceID, Location and number of moves for each peice
        For i = 1 To 32
            objWriter.WriteLine(Convert.ToString(Piece(i)) + ": " + Convert.ToString(Location(i)) + ": " + Convert.ToString(NumOfMoves(i)))
        Next
        'write the number of total moves
        objWriter.WriteLine(TotalMoves)
        If GameMode = "CVH" Then
            'write which player is the human if it is CVH
            objWriter.WriteLine(Reason + "!" + HumPlayer)
        Else
            'write reason for save
            objWriter.WriteLine(Reason)
        End If
        objWriter.Close()
    End Sub
    'loads a game onto all the arrays from a text file
    Sub Load(ByVal FilePath As String)
        Dim lines(8) As String
        'read the board
        For i = 1 To 8
            lines(i) = ReadLineWithNumberFrom(FilePath, i)
        Next
        Dim hp As Integer = 0
        Dim hr As Integer = 0
        Dim hh As Integer = 0
        Dim hb As Integer = 0
        Dim hq As Integer = 0
        Dim hk As Integer = 0
        Dim cp As Integer = 0
        Dim cr As Integer = 0
        Dim ch As Integer = 0
        Dim cb As Integer = 0
        Dim cq As Integer = 0
        Dim ck As Integer = 0
        For i = 1 To 32
            Location(i) = "KO"
        Next
        'go through the board loading everything into the correct places on board, while counting the pieces to check if there is too many
        For y = 1 To 8
            For x = 0 To 7
                Board(x + 1, y) = (lines(y))(x)
                If (lines(y))(x) <> " " Then
                    Select Case ((lines(y))(x))
                        Case "p"
                            hp += 1
                            If hp > 8 Then
                                Console.WriteLine("Wrong number of pawns human side")
                            End If
                        Case "r"
                            hr += 1
                            If hr > 2 Then
                                Console.WriteLine("Wrong number of rooks human side")
                            End If
                        Case "1"
                            hh += 1
                            If hh > 2 Then
                                Console.WriteLine("Wrong number of knights human side")
                            End If
                        Case "b"
                            hb += 1
                            If hb > 2 Then
                                Console.WriteLine("Wrong number of bishops human side")
                            End If
                        Case "q"
                            hq += 1
                            If hq >= 2 Then
                                Console.WriteLine("Wrong number of queens human side")
                            End If
                        Case "k"
                            hk += 1
                            If hk > 2 Then
                                Console.WriteLine("Wrong number of kings human side")
                            End If
                        Case "P"
                            cp += 1
                            If cp > 8 Then
                                Console.WriteLine("Wrong number of pawns computer side")
                            End If
                        Case "R"
                            cr += 1
                            If cr > 2 Then
                                Console.WriteLine("Wrong number of rooks computer side")
                            End If
                        Case "1"
                            ch += 1
                            If ch > 2 Then
                                Console.WriteLine("Wrong number of knights computer side")
                            End If
                        Case "B"
                            cb += 1
                            If cb > 2 Then
                                Console.WriteLine("Wrong number of bishops computer side")
                            End If
                        Case "Q"
                            cq += 1
                            If cq > 2 Then
                                Console.WriteLine("Wrong number of queens computer side")
                            End If
                        Case "K"
                            ck += 1
                            If ck > 2 Then
                                Console.WriteLine("Wrong number of kings computer side")
                            End If
                    End Select
                End If
            Next
        Next
        'loading pieceID, Location and number of moves
        Dim PieceLine(32) As String
        For i = 1 To 32
            Dim Number As Integer = 0
            PieceLine(i) = ReadLineWithNumberFrom(FilePath, 10 + i)
            For o = 10 To Len(PieceLine(i)) - 1
                If IsNumeric(PieceLine(i)(o)) = True Then
                    Number += Asc(PieceLine(i)(o)) - 48
                    NumOfMoves(i) = Number
                    If o <> Len(PieceLine(i)) Then
                        Number = Number * 10
                    End If
                End If
            Next
            Dim Loc As String = ""
            For o = 6 To 7
                Loc += PieceLine(i)(o)
            Next
            Location(i) = Loc
            Dim PieceID As String = ""
            For o = 0 To 3
                PieceID += PieceLine(i)(o)
            Next
            Piece(i) = PieceID
        Next
        'reading the saving reason
        TotalMoves = Convert.ToInt16(ReadLineWithNumberFrom(FilePath, 43))
        Dim Reason As String = ReadLineWithNumberFrom(FilePath, 44)
        LoadingReason = Reason
        'determining if it was a move file or not
        If Reason(0) = "M" Then
            ReviewMoveFrom = Reason(5) + Reason(6)
            ReviewMoveTo = Reason(8) + Reason(9)
        End If
    End Sub
    'read a line from a file
    Function ReadLineWithNumberFrom(ByVal filePath As String, ByVal lineNumber As Integer) As String
        'trys to read the file
        Try
            Using file As New StreamReader(filePath)
                ' Skip all preceding lines: '
                For i As Integer = 1 To lineNumber - 1
                    If file.ReadLine() Is Nothing Then
                        'the line is nothing
                        Throw New ArgumentOutOfRangeException("lineNumber")
                    End If
                Next
                ' Attempt to read the line you're interested in: '
                Dim line As String = file.ReadLine()
                If line Is Nothing Then
                    'the line is nothing
                    Throw New ArgumentOutOfRangeException("lineNumber")
                End If
                ' Succeded!
                Return line
            End Using
        Catch e As Exception
            'cannot read the file
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(e.Message)
            Return ""
        End Try
    End Function
    'refreshs the board based on the location array
    Sub RefreshBoard()
        'clears the board
        For x = 1 To 8
            For y = 1 To 8
                Board(x, y) = " "
            Next
        Next
        'all the white pieces are places
        For i = 1 To 32
            If Location(i) <> "KO" And ToCoordX(Location(i)(0)) >= 1 And ToCoordX(Location(i)(0)) <= 8 And ToCoordY(Location(i)(1)) >= 1 And ToCoordY(Location(i)(1)) <= 8 And ToCoordX(Location(i)(0)) >= 1 And ToCoordX(Location(i)(0)) <= 8 And ToCoordY(Location(i)(1)) >= 1 And ToCoordY(Location(i)(1)) <= 8 Then
                If ToCoordX(Location(i)(0)) >= 1 And ToCoordX(Location(i)(0)) <= 8 And ToCoordY(Location(i)(1)) >= 1 And ToCoordY(Location(i)(1)) <= 8 Then
                    If i > 16 Then
                        'black piece
                        Board(ToCoordX(Location(i)(0)), ToCoordY(Location(i)(1))) = UCase(Piece(i)(3))
                    Else
                        'white piece
                        Board(ToCoordX(Location(i)(0)), ToCoordY(Location(i)(1))) = LCase(Piece(i)(3))
                    End If
                End If
            End If
        Next
    End Sub
    'explains to the user the symbols and the criteria to win or draw
    Sub Intro()
        Console.WriteLine("P = Pawn")
        Console.WriteLine("R = Rook")
        Console.WriteLine("N = Knight")
        Console.WriteLine("B = Bishop")
        Console.WriteLine("Q = Queen")
        Console.WriteLine("K = King")
        Console.WriteLine("CheckMate to Win")
        Console.WriteLine("FiveFold, Stalemate and Insufficient Material Draw are the ways to draw")
        Console.ReadLine()
    End Sub
    'sets up all the arrays
    Sub SetUp()
        'giving values to Xaxis array
        Xaxis(1) = "A"
        Xaxis(2) = "B"
        Xaxis(3) = "C"
        Xaxis(4) = "D"
        Xaxis(5) = "E"
        Xaxis(6) = "F"
        Xaxis(7) = "G"
        Xaxis(8) = "H"
        'giving values to Yaxis arrays
        For i = 1 To 8
            Yaxis(9 - i) = Convert.ToString(i)
        Next
        'giving values to the piece symobols array
        PieceSymbol(0) = "k"
        PieceSymbol(1) = "q"
        PieceSymbol(2) = "b"
        PieceSymbol(3) = "n"
        PieceSymbol(4) = "r"
        PieceSymbol(5) = "p"
    End Sub
    'returns True if a vaule is odd and false if it is even
    Function IsOdd(ByVal inum As Integer) As Boolean
        IsOdd = ((inum \ 2) * 2 <> inum)
    End Function
    'displays the array Board with no colouring
    Sub DisplayBoard()
        'goes from top left to bottom right through the array
        For y = 1 To 8
            For x = 1 To 8
                If x = 8 Then
                    Console.WriteLine(Board(x, y))
                Else
                    Console.Write(Board(x, y))
                End If
            Next
        Next
        Console.ReadLine()
    End Sub
    'prints an empty row when displaying the board with the correct background colours
    Sub DisplayEmptyRow(ByVal y As Integer)
        Dim Colour As Integer
        'prints a bit for the axis
        Console.BackgroundColor = ConsoleColor.Black
        Console.Write("   ")
        'decides on the colour to print
        If IsOdd(y) = True Then
            Colour = 0
        Else
            Colour = 15
        End If
        'goes along the row printing alternating colours
        For x = 1 To 8
            If Colour = 0 Then
                Colour = 15
            Else
                Colour = 0
            End If
            Console.BackgroundColor = Colour
            Console.Write("       ")
        Next
        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine()
    End Sub
    'works out the correct forground colour to print in
    Function ForeColour(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim Temp As Integer = 10
        For i = 1 To 32
            If Location(i) = Xaxis(x) + Yaxis(y) Then
                If i > 16 Then
                    'prints in the whites colour
                    Temp = WhiteColour
                Else
                    'prints in the blacks colour
                    Temp = BlackColour
                End If
            End If
        Next
        Return Temp
    End Function
    'displays the board how it is most often seen
    Sub Display(ByVal Perspective As Char)
        'gets all the pieces in the correct place on board
        RefreshBoard()
        Console.Clear()
        'gets the correct numbers for the perspective asked
        Dim Start As Integer = 8
        Dim Inc As Integer = -1
        If Perspective = "1" Then
            Start = 1
            Inc = 1
        End If
        Dim Colour As Integer = 0
        'goes through printing from top left to bottom right of the perspective asked
        For y = Start To Start + (Inc * 7) Step Inc
            'fill in the square around
            DisplayEmptyRow(y)
            'black or white
            If IsOdd(y) = True Then
                Colour = 0
            Else
                Colour = 15
            End If
            For x = Start To Start + (Inc * 7) Step Inc
                'black or white
                If Colour = 0 Then
                    Colour = 15
                Else
                    Colour = 0
                End If
                Console.BackgroundColor = Colour
                'decide on the colour of the peice
                Dim ForColour As Integer = ForeColour(x, y)
                Console.ForegroundColor = ForColour
                If x = Start Then
                    'print the Y axis
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = SideColour
                    Console.Write(" " + Yaxis(y) + " ")
                    Console.BackgroundColor = Colour
                    Console.ForegroundColor = ForColour
                End If
                'print the board
                Console.Write("   " + UCase(Board(x, y)) + "   ")
            Next
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            'fill in the square around
            DisplayEmptyRow(y)
        Next
        'print the X axis
        Console.ForegroundColor = SideColour
        Console.WriteLine()
        Console.Write("   ")
        For i = Start To Start + (Inc * 7) Step Inc
            If i = Start + (Inc * 7) Then
                Console.WriteLine("   " + Xaxis(i))
            Else
                Console.Write("   " + Xaxis(i) + "   ")
            End If
        Next
        Console.ForegroundColor = 15
        Console.BackgroundColor = 0
        Console.WriteLine()
    End Sub
    'humans move
    Sub HumMove()
        'displays in perspective of the human
        Display(CurrentPlayer)
        Valid = False
        Dim MoveFromUser As String
        Dim MoveToUser As String
        'loops until the coords are valid
        Do
            'user inputs
            Console.WriteLine("What is the coordinates of the piece you want to move?")
            MoveFromUser = UCase(Console.ReadLine())
            Console.WriteLine("What is the coordinates of the place you want to move to?")
            MoveToUser = UCase(Console.ReadLine())
            'REGEX to check if the coordinates are valid
            If RegexCoord.Match(MoveFromUser).Success = True And RegexCoord.Match(MoveToUser).Success = True Then
                Valid = True
            Else
                'the coords are invalid
                Console.WriteLine("Not valid coordinates")
                Console.ReadLine()
            End If
        Loop Until Valid = True
        'move the piece asked
        Move(CurrentPlayer, MoveFromUser, MoveToUser)
        'update the last thing moved for storing in the move file
        LastMoveFrom = MoveFromUser
        LastMoveTo = MoveToUser
        'swap the players around
        If Valid = True Then
            If CurrentPlayer = "1" Then
                CurrentPlayer = "2"
                OP = "1"
            Else
                CurrentPlayer = "1"
                OP = "2"
            End If
        End If
    End Sub
    'converts a x Value of a coordinate to the number in the array
    Function ToCoordX(ByVal XChar As Char) As Integer
        Return Asc(XChar) - 64
    End Function
    'converts a y Value of a coordinate to the number in the array
    Function ToCoordY(ByVal YChar As Char) As Integer
        Return 57 - Asc(YChar)
    End Function
    'checks if the move trying to be made is valid
    Function CheckIfValidMove(ByVal Player As String, ByVal MoveFrom As String, ByVal MoveTo As String) As Boolean
        Dim Temp As Boolean = False
        'get a list of possible moves
        Dim PossibleMoveFrom(0) As String
        Dim PossibleMoveTo(0) As String
        Possible(Player, Board, Location, NumOfMoves, PossibleMoveFrom, PossibleMoveTo, True)
        'go through the list of possible moves to check if the move trying to be made is in there
        For i = 0 To PossibleMoveFrom.Length - 1
            If PossibleMoveFrom(i) = MoveFrom And PossibleMoveTo(i) = MoveTo Then
                'the move is valid
                Temp = True
            End If
        Next
        Return Temp
    End Function
    'moves peices around the board
    Sub Move(ByVal Player As Char, ByVal MoveFrom As String, ByVal MoveTo As String)
        Valid = False
        Dim PlayerValid As Boolean = False
        'checks to see if it is a valid move
        If CheckIfValidMove(Player, MoveFrom, MoveTo) = True Then
            'copys all the arrays to come back to later
            Dim TempBoard(8, 8) As Char
            CopyBoards(Board, TempBoard)
            Dim TempLocation(0) As String
            CopyStrArr(Location, TempLocation)
            Dim TempNumOfMoves(0) As Integer
            CopyIntArr(NumOfMoves, TempNumOfMoves)
            Dim TempPieces(0) As String
            CopyStrArr(Piece, TempPieces)
            'simulates the move on te copys
            SimMove(TempBoard, TempLocation, TempNumOfMoves, MoveFrom, MoveTo, TempPieces)
            'checks to see if the player is in check after the move
            If InCheck(Player, TempBoard, TempLocation, TempNumOfMoves) = False Then
                'checks to see if it is the computer moving
                If (Player = "1" And Player1Strat = "C") Or (Player = "2" And Player2Strat = "C") Then
                    'if it is checks to see if it has been in the same boardstate 3 times
                    If MoveInDraw(3, TempBoard) = True Then
                        'if it has then add it to the tried before moves which will make sure that those moves dont come up in the possible outcome
                        Array.Resize(TriedMoveFrom, TriedMoveFrom.Length + 1)
                        Array.Resize(TriedMoveTo, TriedMoveTo.Length + 1)
                        TriedMoveFrom(TriedMoveFrom.Length - 1) = MoveFrom
                        TriedMoveTo(TriedMoveTo.Length - 1) = MoveTo
                    Else
                        PlayerValid = True
                    End If
                Else
                    PlayerValid = True
                End If
            Else
                'if in check then add it to the tried before moves
                Array.Resize(TriedMoveFrom, TriedMoveFrom.Length + 1)
                Array.Resize(TriedMoveTo, TriedMoveTo.Length + 1)
                TriedMoveFrom(TriedMoveFrom.Length - 1) = MoveFrom
                TriedMoveTo(TriedMoveTo.Length - 1) = MoveTo
            End If
        End If
        'checking what to move now that it has been established that it is all correct other than castling
        If PlayerValid = True Then
            'sort out castling
            If UCase(Board(ToCoordX(MoveFrom(0)), ToCoordY(MoveFrom(1)))) = "K" Then
                Dim Start As Integer = 1
                If Player = "2" Then
                    Start = 17
                End If
                'checking that the player isnt it check
                If InCheck(Player, Board, Location, NumOfMoves) = False Then
                    'moving both peices
                    If ToCoordX(MoveFrom(0)) - 2 = ToCoordX(MoveTo(0)) And MoveFrom(1) = MoveTo(1) Then
                        Location(Start + 8) = "D" + Location(Start + 8)(1)
                        Location(Start + 12) = "C" + Location(Start + 12)(1)
                        NumOfMoves(Start + 12) += 1
                    ElseIf ToCoordX(MoveFrom(0)) + 2 = ToCoordX(MoveTo(0)) And MoveFrom(1) = MoveTo(1) Then
                        Location(Start + 15) = "F" + Location(Start + 8)(1)
                        Location(Start + 12) = "G" + Location(Start + 12)(1)
                        NumOfMoves(Start + 12) += 1
                    Else
                        'if they are not castling moving the king normally
                        If Board(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) <> " " Then
                            'KOing the peice in the way
                            For i = 1 To 32
                                If Location(i) = MoveTo Then
                                    Location(i) = "KO"
                                End If
                            Next
                        End If
                        'moving the king
                        Location(Start + 12) = MoveTo
                        NumOfMoves(Start + 12) += 1
                    End If
                End If
                'checking if they are moving a pawn
            ElseIf UCase(Board(ToCoordX(MoveFrom(0)), ToCoordY(MoveFrom(1)))) = "P" Then
                If LastMoveFrom <> "" Then
                    'determing if the last move was a pawn
                    If UCase(Board(ToCoordX(LastMoveTo(0)), ToCoordY(LastMoveTo(1)))) = "P" Then
                        'deciding which way is forward
                        Dim Forward As Integer = -1
                        Dim EndBoard As Integer = 8
                        If Player = "2" Then
                            Forward = 1
                            EndBoard = 1
                        End If
                        'checking all the conditions for en passant
                        If ToCoordY(LastMoveFrom(1)) = EndBoard - Forward And ToCoordY(LastMoveTo(1)) = EndBoard - (3 * Forward) Then
                            If MoveTo(0) = LastMoveFrom(0) And ToCoordY(MoveTo(1)) = ToCoordY(LastMoveFrom(1)) + Forward Then
                                If Board(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) = " " Then
                                    For i = 1 To 32
                                        If Location(i) = LastMoveTo Then
                                            Location(i) = "KO"
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
                'if the locationg of the move to isnt empty for the pawn
                If Board(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) <> " " Then
                    'KO whatever was there
                    For i = 1 To 32
                        If Location(i) = MoveTo Then
                            Location(i) = "KO"
                        End If
                    Next
                End If
                'move the pawn
                For i = 1 To 32
                    If Location(i) = MoveFrom Then
                        Location(i) = MoveTo
                        NumOfMoves(i) += 1
                    End If
                Next
            Else
                'for everything else
                If Board(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) <> " " Then
                    'KO what was in the move to place
                    For i = 1 To 32
                        If Location(i) = MoveTo Then
                            Location(i) = "KO"
                        End If
                    Next
                End If
                'move there
                For i = 1 To 32
                    If Location(i) = MoveFrom Then
                        Location(i) = MoveTo
                        NumOfMoves(i) += 1
                    End If
                Next
            End If
            Valid = True
            TotalMoves += 1
            'reset tried before
            Array.Resize(TriedMoveFrom, 0)
            Array.Resize(TriedMoveTo, 0)
        Else
            'when it isnt a valid move and when there is a human to read it
            If GameMode = "CVH" Or GameMode = "HVH" Then
                Console.WriteLine(MoveFrom + " to " + MoveTo + " is not a valid move")
                Console.ReadKey()
            End If
            Valid = False
        End If
        'promotion
        If GameMode <> "NA" Then
            'determine the correct edge and indexs
            Dim UPStart As Integer = 17
            Dim Edge As String = "1"
            Dim MateNumber As Integer = 0
            Dim AllNumber As Integer = 0
            If CurrentPlayer = "1" Then
                UPStart = 1
                Edge = "8"
            End If
            For i = UPStart To UPStart + 7
                'if the pawn is at the edge
                If Location(i)(1) = Edge And LCase(Piece(i)(3)) = "p" Then
                    If (CurrentPlayer = "1" And Player1Strat = "H") Or (CurrentPlayer = "2" And Player2Strat = "H") Then
                        'menu for promotion if it is a human
                        Dim Again As Boolean = True
                        Dim Current As String = Piece(i)
                        Console.Clear()
                        Dim Entry As String = "UpArrow"
                        Dim k As ConsoleKeyInfo
                        Dim Selected As Integer = 2
                        Do
                            Select Case (Entry)
                                Case "UpArrow"
                                    If Not Selected = 1 Then
                                        Selected = Selected - 1
                                    End If
                                Case "DownArrow"
                                    If Not Selected = 4 Then
                                        Selected = Selected + 1
                                    End If
                            End Select
                            Do
                                Console.Clear()
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("What piece type do you want to turn your pawn on " + Location(i) + " to")
                                If Selected = 1 Then
                                    Console.ForegroundColor = ConsoleColor.Magenta
                                Else
                                    Console.ForegroundColor = ConsoleColor.White
                                End If
                                Console.WriteLine("Rook")
                                If Selected = 2 Then
                                    Console.ForegroundColor = ConsoleColor.Magenta
                                Else
                                    Console.ForegroundColor = ConsoleColor.White
                                End If
                                Console.WriteLine("Knight")
                                If Selected = 3 Then
                                    Console.ForegroundColor = ConsoleColor.Magenta
                                Else
                                    Console.ForegroundColor = ConsoleColor.White
                                End If
                                Console.WriteLine("Bishop")
                                If Selected = 4 Then
                                    Console.ForegroundColor = ConsoleColor.Magenta
                                Else
                                    Console.ForegroundColor = ConsoleColor.White
                                End If
                                Console.WriteLine("Queen")
                                k = Console.ReadKey(True)
                                Entry = k.Key.ToString
                            Loop Until Entry = "DownArrow" Or Entry = "UpArrow" Or Entry = "Enter"
                        Loop Until Entry = "Enter"
                        'change hte pawn ID to the desired piece
                        Select Case (Selected)
                            Case 1
                                Current.Replace("p", "r")
                            Case 2
                                Current.Replace("p", "1")
                            Case 3
                                Current.Replace("p", "b")
                            Case 4
                                Current.Replace("p", "q")
                        End Select
                        Piece(i) = Current
                    Else
                        'but if it is a computer
                        Dim Current As String = Piece(i)
                        'determine the player
                        Dim KnightPossibleMoveFrom(0) As String
                        Dim KnightPossibleMoveTo(0) As String
                        Dim DiffASC As Integer = 32
                        OP = "2"
                        If CurrentPlayer = "2" Then
                            DiffASC = 0
                            OP = "1"
                        End If
                        'get all the possible moves for the opponent
                        Dim PossibleOPMoveFrom(0) As String
                        Dim PossibleOPMoveTo(0) As String
                        Possible(OP, Board, Location, NumOfMoves, PossibleOPMoveFrom, PossibleOPMoveTo, True)
                        'go through the possible moves
                        For o = 1 To PossibleOPMoveFrom.Length - 1
                            'copy all the variables
                            Dim BoardAfterOP(8, 8) As Char
                            CopyBoards(Board, BoardAfterOP)
                            Dim LocationAfterOPMove(0) As String
                            CopyStrArr(Location, LocationAfterOPMove)
                            Dim NumOfMoveAfterOPMove(0) As Integer
                            CopyIntArr(NumOfMoves, NumOfMoveAfterOPMove)
                            Dim TempPieces(0) As String
                            CopyStrArr(Piece, TempPieces)
                            'simulate the moves
                            SimMove(BoardAfterOP, LocationAfterOPMove, NumOfMoveAfterOPMove, PossibleOPMoveFrom(o), PossibleOPMoveTo(o), TempPieces)
                            'get all the possible knight moves
                            For x = -2 To 2 Step 4
                                For y = -1 To 1 Step 2
                                    If (ToCoordX(Location(i)(0)) + x >= 1 And ToCoordX(Location(i)(0)) + x <= 8 And ToCoordY(Location(i)(1)) + y <= 8 And ToCoordY(Location(i)(1)) + y >= 1) Then
                                        For p = 0 To 5
                                            If Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = " " Then
                                                PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, True)
                                            ElseIf Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = Chr(Asc(PieceSymbol(p)) - DiffASC) Then
                                                PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, True)
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                            For y = -2 To 2 Step 4
                                For x = -1 To 1 Step 2
                                    If (ToCoordX(Location(i)(0)) + x >= 1 And ToCoordX(Location(i)(0)) + x <= 8 And ToCoordY(Location(i)(1)) + y <= 8 And ToCoordY(Location(i)(1)) + y >= 1) Then
                                        For p = 0 To 5
                                            If Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = " " Then
                                                PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, True)
                                            ElseIf Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = Chr(Asc(PieceSymbol(p)) - DiffASC) Then
                                                PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, True)
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                            'go through them
                            For p = 1 To KnightPossibleMoveFrom.Length - 1
                                'copy the arrays
                                Dim BoardAfterMove(8, 8) As Char
                                CopyBoards(BoardAfterOP, BoardAfterMove)
                                Dim LocationAfterMove(0) As String
                                CopyStrArr(LocationAfterOPMove, LocationAfterMove)
                                Dim NumOfMoveAfterMove(0) As Integer
                                CopyIntArr(NumOfMoveAfterOPMove, NumOfMoveAfterMove)
                                'simulate the move
                                SimMove(BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove, KnightPossibleMoveFrom(p), KnightPossibleMoveTo(p), TempPieces)
                                'count the times it is checkmate
                                If CheckMated(BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove, OP, Piece) = True Then
                                    MateNumber += 1
                                End If
                                AllNumber += 1
                            Next
                        Next
                        'if it was checkmate all the time chage to a knight
                        If MateNumber = AllNumber Then
                            Current = Current.Replace("p", "n")
                        Else
                            'otherwise change to a queen
                            Current = Current.Replace("p", "q")
                        End If
                        'change the piece
                        Piece(i) = Current
                    End If
                End If
            Next
            'save the compressed move
            Using writer As New StreamWriter(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\CompressedMove.txt", True)
                writer.WriteLine(BoardLine(Board))
            End Using
        End If
        'refresh the board
        RefreshBoard()
    End Sub
    'copy a 8,8 2D char array into another
    Sub CopyBoards(ByVal Ori(,) As Char, ByRef Copy(,) As Char)
        'go from top left to bottom right down the sides
        For x = 1 To 8
            For y = 1 To 8
                Copy(x, y) = Ori(x, y)
            Next
        Next
    End Sub
    'copy a 1D array of integers into another
    Sub CopyIntArr(ByVal Original() As Integer, ByRef Copy() As Integer)
        'resize the array to be copied into
        Array.Resize(Copy, Original.Length)
        'copy all the elements over
        For i = 0 To Original.Length - 1
            Copy(i) = Original(i)
        Next
    End Sub
    'copy a 1D string aray into another
    Sub CopyStrArr(ByVal Original() As String, ByRef Copy() As String)
        'resize the array to be copied into
        Array.Resize(Copy, Original.Length)
        'copy all the elements across
        For i = 0 To Original.Length - 1
            Copy(i) = Original(i)
        Next
    End Sub
    'simulates a move on the board passed in
    Sub SimMove(ByRef TempBoard(,) As Char, ByRef TempLocation() As String, ByRef TempNumOfMoves() As Integer, ByVal MoveFrom As String, ByVal MoveTo As String, ByRef TempPieces() As String)
        'get all the numbers of where to move stuff over set up
        Dim MoveToXCord As Integer = ToCoordX(MoveTo(0))
        Dim MoveToYCord As Integer = ToCoordY(MoveTo(1))
        Dim MoveFromXCord As Integer = ToCoordX(MoveFrom(0))
        Dim MoveFromYCord As Integer = ToCoordY(MoveFrom(1))
        'replace the element that was at the move too
        TempBoard(MoveToXCord, MoveToYCord) = TempBoard(MoveFromXCord, MoveFromYCord)
        'delete the space were it was
        TempBoard(MoveFromXCord, MoveFromYCord) = " "
        'simulated king move
        If UCase(TempBoard(ToCoordX(MoveFrom(0)), ToCoordY(MoveFrom(1)))) = "K" Then
            Dim Start As Integer = 1
            Dim Player As Char = "1"
            For i = 1 To 32
                If Location(i) = MoveFrom Then
                    If i > 16 Then
                        Player = "2"
                    End If
                End If
            Next
            If Player = "2" Then
                Start = 17
            End If
            'simulate castling
            If ToCoordX(MoveFrom(0)) - 2 = ToCoordX(MoveTo(0)) And MoveFrom(1) = MoveTo(1) Then
                'move the pieces
                TempLocation(Start + 8) = "D" + TempLocation(Start + 8)(1)
                TempLocation(Start + 12) = "H" + TempLocation(Start + 12)(1)
                TempNumOfMoves(Start + 12) += 1
            ElseIf ToCoordX(MoveFrom(0)) + 2 = ToCoordX(MoveTo(0)) And MoveFrom(1) = MoveTo(1) Then
                'moves the pieces
                TempLocation(Start + 15) = "F" + TempLocation(Start + 8)(1)
                TempLocation(Start + 12) = "G" + TempLocation(Start + 12)(1)
                TempNumOfMoves(Start + 12) += 1
            Else
                'moves the king if not castling
                If TempBoard(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) <> " " Then
                    'KO the thing in its move to
                    For i = 1 To 32
                        If TempLocation(i) = MoveTo Then
                            TempLocation(i) = "KO"
                        End If
                    Next
                End If
                'moves the king
                If TempLocation(Start + 12) = MoveFrom Then
                    TempLocation(Start + 12) = MoveTo
                    TempNumOfMoves(Start + 12) += 1
                End If
            End If
        Else
            'moves anything else
            If TempBoard(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1))) <> " " Then
                'KO the move to element
                For i = 1 To 32
                    If TempLocation(i) = MoveTo Then
                        TempLocation(i) = "KO"
                    End If
                Next
            End If
            'move the move from element
            For i = 1 To 32
                If TempLocation(i) = MoveFrom Then
                    TempLocation(i) = MoveTo
                    TempNumOfMoves(i) += 1
                End If
            Next
        End If
        'simulate promotion
        If UCase(TempBoard(ToCoordX(MoveTo(0)), ToCoordY(MoveTo(1)))) = "P" Then
            'checking if the pawwn was at the edge
            If ToCoordY(MoveTo(1)) = 8 Or ToCoordY(MoveTo(1)) = 1 Then
                Dim MateNumber As Integer = 0
                Dim AllNumber As Integer = 0
                'get the piece index 
                Dim i As Integer
                For a = 1 To 32
                    If TempLocation(a) = MoveTo Then
                        i = a
                    End If
                Next
                'get the player
                Dim Player As Char = "1"
                If i > 16 Then
                    Player = "2"
                End If
                'all the variables to do with the player
                Dim Current As String = Piece(i)
                Dim KnightPossibleMoveFrom(0) As String
                Dim KnightPossibleMoveTo(0) As String
                Dim DiffASC As Integer = 32
                OP = "2"
                If CurrentPlayer = "2" Then
                    DiffASC = 0
                    OP = "1"
                End If
                'get the possible array of moves for the opponent
                Dim PossibleOPMoveFrom(0) As String
                Dim PossibleOPMoveTo(0) As String
                Possible(OP, TempBoard, TempLocation, TempNumOfMoves, PossibleOPMoveFrom, PossibleOPMoveTo, True)
                'go through them
                For o = 1 To PossibleOPMoveFrom.Length - 1
                    'copy the variables to come back to later
                    Dim BoardAfterOP(8, 8) As Char
                    CopyBoards(TempBoard, BoardAfterOP)
                    Dim LocationAfterOPMove(0) As String
                    CopyStrArr(TempLocation, LocationAfterOPMove)
                    Dim NumOfMoveAfterOPMove(0) As Integer
                    CopyIntArr(TempNumOfMoves, NumOfMoveAfterOPMove)
                    Dim PiecesAfter(0) As String
                    CopyStrArr(TempPieces, PiecesAfter)
                    'simulate the move
                    SimMove(BoardAfterOP, LocationAfterOPMove, NumOfMoveAfterOPMove, PossibleOPMoveFrom(o), PossibleOPMoveTo(o), PiecesAfter)
                    'knight possible moves
                    For x = -2 To 2 Step 4
                        For y = -1 To 1 Step 2
                            If (ToCoordX(Location(i)(0)) + x >= 1 And ToCoordX(Location(i)(0)) + x <= 8 And ToCoordY(Location(i)(1)) + y <= 8 And ToCoordY(Location(i)(1)) + y >= 1) Then
                                For p = 0 To 5
                                    If Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = " " Then
                                        PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, False)
                                    ElseIf Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = Chr(Asc(PieceSymbol(p)) - DiffASC) Then
                                        PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, False)
                                    End If
                                Next
                            End If
                        Next
                    Next
                    For y = -2 To 2 Step 4
                        For x = -1 To 1 Step 2
                            If (ToCoordX(Location(i)(0)) + x >= 1 And ToCoordX(Location(i)(0)) + x <= 8 And ToCoordY(Location(i)(1)) + y <= 8 And ToCoordY(Location(i)(1)) + y >= 1) Then
                                For p = 0 To 5
                                    If Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = " " Then
                                        PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, False)
                                    ElseIf Board(ToCoordX(Location(i)(0)) + x, ToCoordY(Location(i)(1)) + y) = Chr(Asc(PieceSymbol(p)) - DiffASC) Then
                                        PossibleAdd(KnightPossibleMoveFrom, KnightPossibleMoveTo, x, y, i, Board, Location, NumOfMoves, Player, False)
                                    End If
                                Next
                            End If
                        Next
                    Next
                    'go through the knights possible moves
                    For p = 1 To KnightPossibleMoveFrom.Length - 1
                        'copy the boards
                        Dim BoardAfterMove(8, 8) As Char
                        CopyBoards(BoardAfterOP, BoardAfterMove)
                        Dim LocationAfterMove(0) As String
                        CopyStrArr(LocationAfterOPMove, LocationAfterMove)
                        Dim NumOfMoveAfterMove(0) As Integer
                        CopyIntArr(NumOfMoveAfterOPMove, NumOfMoveAfterMove)
                        'simulate the move
                        SimMove(BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove, KnightPossibleMoveFrom(p), KnightPossibleMoveTo(p), TempPieces)
                        'see if they are checkmated, count
                        If CheckMated(BoardAfterMove, LocationAfterMove, NumOfMoveAfterMove, OP, Piece) = True Then
                            MateNumber += 1
                        End If
                        AllNumber += 1
                    Next
                Next
                If MateNumber = AllNumber Then
                    'if all the numbers are checkmated then promote to knight
                    Current = Current.Replace("p", "n")
                Else
                    'otherwise promote to queen
                    Current = Current.Replace("p", "q")
                End If
                'change the ID to the promotion
                Piece(i) = Current
            End If
        End If
    End Sub
    'evaluate the score of the pieces from a POV in a given board situation
    Function EvaluateScore(ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByVal TempPieces() As String) As Double
        'get opponets number
        OP = "1"
        If CurrentPlayer = "1" Then
            OP = "2"
        End If
        Dim Score As Integer
        Dim PawnCoefficent As Integer
        Dim RookCoefficent As Integer
        Dim KnightCoefficent As Integer
        Dim BishopCoefficent As Integer
        Dim QueenCoefficent As Integer
        Dim MoveNumberCoefficent As Double
        Dim ProtectedPiecesCoefficent As Double
        Dim ThreatenedPiecesCoefficent As Double
        'get the correct coefficients to evaluate with in the case of CVC
        If CurrentPlayer = "1" Then
            PawnCoefficent = Pawn1Coefficent
            RookCoefficent = Rook1Coefficent
            KnightCoefficent = Knight1Coefficent
            BishopCoefficent = Bishop1Coefficent
            QueenCoefficent = Queen1Coefficent
            MoveNumberCoefficent = Move1NumberCoefficent
            ProtectedPiecesCoefficent = ProtectedPieces1Coefficent
            ThreatenedPiecesCoefficent = ThreatenedPieces1Coefficent
        Else
            PawnCoefficent = Pawn2Coefficent
            RookCoefficent = Rook2Coefficent
            KnightCoefficent = Knight2Coefficent
            BishopCoefficent = Bishop2Coefficent
            QueenCoefficent = Queen2Coefficent
            MoveNumberCoefficent = Move2NumberCoefficent
            ProtectedPiecesCoefficent = ProtectedPieces2Coefficent
            ThreatenedPiecesCoefficent = ThreatenedPieces2Coefficent
        End If
        'adding up the material part of the score
        For y = 1 To 8
            For x = 1 To 8
                If TempBoard(y, x) <> " " Then
                    Select Case (TempBoard(y, x))
                        Case "p"
                            If HumPlayer = "1" Then
                                Score -= PawnCoefficent
                            Else
                                Score += PawnCoefficent
                            End If
                        Case "r"
                            If HumPlayer = "1" Then
                                Score -= RookCoefficent
                            Else
                                Score += RookCoefficent
                            End If
                        Case "n"
                            If HumPlayer = "1" Then
                                Score -= KnightCoefficent
                            Else
                                Score += KnightCoefficent
                            End If
                        Case "b"
                            If HumPlayer = "1" Then
                                Score -= BishopCoefficent
                            Else
                                Score += BishopCoefficent
                            End If
                        Case "q"
                            If HumPlayer = "1" Then
                                Score -= QueenCoefficent
                            Else
                                Score += QueenCoefficent
                            End If
                        Case "k"
                            If HumPlayer = "1" Then
                                Score -= KingCoefficent
                            Else
                                Score += KingCoefficent
                            End If
                        Case "P"
                            If HumPlayer = "1" Then
                                Score += PawnCoefficent
                            Else
                                Score -= PawnCoefficent
                            End If
                        Case "R"
                            If HumPlayer = "1" Then
                                Score += RookCoefficent
                            Else
                                Score -= RookCoefficent
                            End If
                        Case "N"
                            If HumPlayer = "1" Then
                                Score += KnightCoefficent
                            Else
                                Score -= KnightCoefficent
                            End If
                        Case "B"
                            If HumPlayer = "1" Then
                                Score += BishopCoefficent
                            Else
                                Score -= BishopCoefficent
                            End If
                        Case "Q"
                            If HumPlayer = "1" Then
                                Score += QueenCoefficent
                            Else
                                Score -= QueenCoefficent
                            End If
                        Case "K"
                            If HumPlayer = "1" Then
                                Score += KingCoefficent
                            Else
                                Score -= KingCoefficent
                            End If
                    End Select
                End If
            Next
        Next
        'adding everything else that the score takes into account
        'number of possible moves
        Score += MoveNumberCoefficent * (NumberOfPossibles(CurrentPlayer, TempBoard, TempLocation, TempNumOfMoves))
        Score -= MoveNumberCoefficent * (NumberOfPossibles(OP, TempBoard, TempLocation, TempNumOfMoves))
        'protected pieces
        Score += ProtectedPiecesCoefficent * (PPV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, CurrentPlayer))
        Score -= ProtectedPiecesCoefficent * (PPV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, OP))
        'threatened peices
        Score += ThreatenedPiecesCoefficent * (TPV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, CurrentPlayer))
        Score -= ThreatenedPiecesCoefficent * (TPV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, OP))
        'threatened uprotected pieces
        Score += ThreatenedPiecesCoefficent * (TUV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, CurrentPlayer))
        Score -= ThreatenedPiecesCoefficent * (TUV(TempBoard, TempNumOfMoves, TempLocation, TempPieces, OP))
        'if the end game is near
        If NearEndGame = True Then
            Score += 10 * KingMoves(TempBoard, TempNumOfMoves, TempLocation, TempPieces, CurrentPlayer)
            Score -= 10 * KingMoves(TempBoard, TempNumOfMoves, TempLocation, TempPieces, OP)
        End If
        'reverse the score if it is a human player so that it best predicts the humans bets minimax move
        If CurrentPlayer = HumPlayer Then
            Score = Score * -1
        End If
        Return Score
    End Function
    'gets the number of moves that the king can do
    Function KingMoves(ByVal TempBoard(,) As Char, TempNumOfMoves() As Integer, ByVal TempLocation() As String, ByVal TempPieces() As String, ByVal Player As Char)
        Dim Number As Integer = 0
        'getting the correct variables for the player
        Dim start As Integer = 1
        Dim DiffASc As Integer = 0
        Dim OPs As Char = "2"
        If Player = "2" Then
            DiffASc = 32
            OPs = "1"
            start = 17
        End If
        ' getting the kings location
        Dim KL As String = TempLocation(start + 12)
        Dim arr(0) As String
        'searching around the king to find any available location for the king
        For y = -1 To 1
            For x = -1 To 1
                If Not y = 0 And x = 0 Then
                    If ToCoordX(KL(0)) + x <= 8 And ToCoordX(KL(0)) + x >= 1 And ToCoordY(KL(1)) + y <= 8 And ToCoordY(KL(1)) + y >= 1 Then
                        If TempBoard(ToCoordX(KL(0)) + x, ToCoordY(KL(1)) + y) <> " " Then
                            For o = 0 To 5
                                If TempBoard(ToCoordX(KL(0)) + x, ToCoordY(KL(1)) + y) = Chr(Asc(PieceSymbol(o)) - DiffASc) Then
                                    Array.Resize(arr, arr.Length + 1)
                                    arr(arr.Length - 1) = Xaxis(ToCoordX(KL(0)) + x) + Yaxis(ToCoordY(KL(1)) + y)
                                End If
                            Next
                        Else
                            Array.Resize(arr, arr.Length + 1)
                            arr(arr.Length - 1) = Xaxis(ToCoordX(KL(0)) + x) + Yaxis(ToCoordY(KL(1)) + y)
                        End If
                    End If
                End If
            Next
        Next
        'getting the possible moves
        Dim OpsMoveFrom(0) As String
        Dim OpsMoveTo(0) As String
        'cross checking the locations against the possible moves of the opponents 
        Possible(OP, TempBoard, TempLocation, TempNumOfMoves, OpsMoveFrom, OpsMoveTo, False)
        If arr.Length > 1 Then
            'go through the array of spaces around the king
            For i = 1 To arr.Length - 1
                If OpsMoveFrom.Length > 1 Then
                    Dim Found = False
                    'go through the move to locations of the oppostions
                    For o = 1 To OpsMoveFrom.Length - 1
                        If arr(i) = OpsMoveTo(o) Then
                            'the quare is threatened
                            Found = True
                            Exit For
                        End If
                    Next
                    If Found = False Then
                        'the spaces isnt threatened
                        Number += 1
                    End If
                Else
                    'the spaces isnt threatened
                    Number += 1
                End If
            Next
        End If
        Return Number
    End Function
    'calcuate the number of possibe moves
    Function NumberOfPossibles(ByVal Player As Char, ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer) As Integer
        Dim Number As Integer = 0
        'get the number of possible moves
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        Possible(Player, TempBoard, TempLocation, TempNumOfMoves, MoveFrom, MoveTo, True)
        'the length of the array is the number of moves
        Number = MoveFrom.Length - 1
        Return Number
    End Function
    Sub CanPossiblePawnTake(ByVal DiffASc As Integer, ByVal TempBoard(,) As Char, ByVal TempLocation As String, ByVal x As Integer, ByVal Forward As Integer, ByRef Score As Integer)
        'Check to see if the square diagonally infront has a opponents peice in
        If TempBoard(ToCoordX(TempLocation(0)) + x, ToCoordY(TempLocation(1)) + Forward) <> " " Then
            For o = 0 To 5
                If TempBoard(ToCoordX(TempLocation(0)) + x, ToCoordY(TempLocation(1)) + Forward) = Chr(Asc(PieceSymbol(o)) - DiffASc) Then
                    Score += 1
                End If
            Next
        End If
    End Sub
    Sub CanRBQMoves(ByVal DiffASc As Integer, ByVal TempBoard(,) As Char, ByVal TempLocation As String, ByVal x As Integer, ByVal y As Integer, ByRef Score As Integer)
        'set up variables
        Dim Length As Integer = 0
        Dim FirstTake As Boolean = False
        Dim Blockage As Boolean = False
        Dim Outside As Boolean = False
        Do
            Length += 1
            'check if it is still on the board
            If ToCoordX(TempLocation(0)) + (Length * x) <= 8 And ToCoordX(TempLocation(0)) + (Length * x) >= 1 And ToCoordY(TempLocation(1)) + (Length * y) <= 8 And ToCoordY(TempLocation(1)) + (Length * y) >= 1 Then
                'check if a piece is there
                If TempBoard(ToCoordX(TempLocation(0)) + (Length * x), ToCoordY(TempLocation(1)) + (Length * y)) = " " Then
                    Score += 1
                Else
                    'check for opponents piece
                    For o = 0 To 5
                        If TempBoard(ToCoordX(TempLocation(0)) + (Length * x), ToCoordY(TempLocation(1)) + (Length * y)) = Chr(Asc(PieceSymbol(o)) - DiffASc) Then
                            Score += 1
                            FirstTake = True
                        Else
                            Blockage = True
                        End If
                    Next
                End If
            Else
                Outside = True
            End If
        Loop Until FirstTake = True Or Blockage = True Or Outside = True
    End Sub
    'return true if in check false if not
    Function InCheck(ByVal Player As Char, ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer)
        Dim TorF As Boolean = False
        'get oppostion
        Dim Opposition As Char = "2"
        Dim Start As Integer = 1
        If Player = "2" Then
            Opposition = "1"
            Start = 17
        End If
        'get oppostions possibles
        Dim OPMF(0) As String
        Dim OPMT(0) As String
        Possible(Opposition, TempBoard, TempLocation, TempNumOfMoves, OPMF, OPMT, True)
        'go through the possibles
        For i = 1 To OPMF.Length - 1
            'see if they threaten the king
            If OPMT(i) = TempLocation(Start + 12) Then
                TorF = True
            End If
        Next
        Return TorF
    End Function
    'populated the two arrays with move from and move to coords
    Sub Possible(ByVal Player As Char, ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByRef PossibleMoveFrom() As String, ByRef PossibleMoveTo() As String, ByVal IgnTriedBefore As Boolean)
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        'get the starting numbers
        Dim Start As Integer = 1
        Dim DiffASC As Integer = 32
        If Player = "2" Then
            Start = 17
            DiffASC = 0
        End If
        For i = Start To Start + 15
            'check if its for the right player and not dead
            If TempLocation(i) <> "KO" Then
                Select Case (LCase(Piece(i)(3)))
                    Case "p"
                        'get the forward and end board based on the player number
                        Dim Forward As Integer = -1
                        Dim EndBoard As Integer = 8
                        If Player = "2" Then
                            Forward = 1
                            EndBoard = 1
                        End If
                        If ToCoordY(TempLocation(i)(1)) <> EndBoard Then
                            'Taking diagonally
                            If ToCoordX(TempLocation(i)(0)) = 1 Then
                                PossiblePawnTake(DiffASC, TempBoard, i, MoveFrom, MoveTo, 1, Forward, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                            ElseIf ToCoordX(TempLocation(i)(0)) = 8 Then
                                PossiblePawnTake(DiffASC, TempBoard, i, MoveFrom, MoveTo, -1, Forward, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                            Else
                                For x = -1 To 1 Step 2
                                    PossiblePawnTake(DiffASC, TempBoard, i, MoveFrom, MoveTo, x, Forward, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                Next
                            End If
                            'Forwards
                            If TempNumOfMoves(i) = 0 Then
                                'Two Forward
                                If TempBoard(ToCoordX(TempLocation(i)(0)), ToCoordY(TempLocation(i)(1)) + (2 * Forward)) = " " And TempBoard(ToCoordX(TempLocation(i)(0)), ToCoordY(TempLocation(i)(1)) + Forward) = " " Then
                                    PossibleAdd(MoveFrom, MoveTo, 0, 2 * Forward, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                End If
                            End If
                            'One Forward
                            If ToCoordX(TempLocation(i)(0)) >= 1 And ToCoordX(TempLocation(i)(0)) <= 8 Then
                                If ToCoordY(TempLocation(i)(1)) + Forward >= 1 And ToCoordY(TempLocation(i)(1)) + Forward <= 8 Then
                                    If TempBoard(ToCoordX(TempLocation(i)(0)), ToCoordY(TempLocation(i)(1)) + Forward) = " " Then
                                        PossibleAdd(MoveFrom, MoveTo, 0, Forward, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        If TempNumOfMoves(i) = 0 Then
                                            'Two Forward
                                            If ToCoordY(TempLocation(i)(1)) + 2 * Forward >= 1 And ToCoordY(TempLocation(i)(1)) + 2 * Forward <= 8 Then
                                                If TempBoard(ToCoordX(TempLocation(i)(0)), ToCoordY(TempLocation(i)(1)) + (2 * Forward)) = " " Then
                                                    PossibleAdd(MoveFrom, MoveTo, 0, 2 * Forward, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        'en passent
                        If LastMoveFrom <> "" Then
                            If Board(ToCoordX(LastMoveTo(0)), ToCoordY(LastMoveTo(1))) = "P" Then
                                If ToCoordY(LastMoveFrom(1)) = EndBoard - Forward And ToCoordY(LastMoveTo(1)) = EndBoard - (3 * Forward) Then
                                    If ToCoordX(LastMoveFrom(0)) > ToCoordX(TempLocation(0)) Then
                                        PossibleAdd(MoveFrom, MoveTo, -1, Forward, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                    Else
                                        PossibleAdd(MoveFrom, MoveTo, 1, Forward, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                    End If
                                End If
                            End If
                        End If
                    Case "r"
                        'go through the rooks lines
                        For o = -1 To 1 Step 2
                            RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, o, 0, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                            RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 0, o, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        Next
                    Case "n"
                        'check the locations that the knight can go too
                        Dim TempKnight As Boolean = False
                        For x = -2 To 2 Step 4
                            For y = -1 To 1 Step 2
                                If (ToCoordX(TempLocation(i)(0)) + x >= 1 And ToCoordX(TempLocation(i)(0)) + x <= 8 And ToCoordY(TempLocation(i)(1)) + y <= 8 And ToCoordY(TempLocation(i)(1)) + y >= 1) Then
                                    For o = 0 To 5
                                        If TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) = " " Then
                                            PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        ElseIf TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) = Chr(Asc(PieceSymbol(o)) - DiffASC) Then
                                            PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        End If
                                    Next
                                End If
                            Next
                        Next
                        For y = -2 To 2 Step 4
                            For x = -1 To 1 Step 2
                                If (ToCoordX(TempLocation(i)(0)) + x >= 1 And ToCoordX(TempLocation(i)(0)) + x <= 8 And ToCoordY(TempLocation(i)(1)) + y <= 8 And ToCoordY(TempLocation(i)(1)) + y >= 1) Then
                                    For o = 0 To 5
                                        If TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) = " " Then
                                            PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        ElseIf TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) = Chr(Asc(PieceSymbol(o)) - DiffASC) Then
                                            PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        End If
                                    Next
                                End If
                            Next
                        Next
                    Case "b"
                        'go through the lines of a bishop
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 1, 1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 1, -1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, -1, -1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, -1, 1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                    Case "q"
                        'Rooks
                        For o = -1 To 1 Step 2
                            RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, o, 0, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                            RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 0, o, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        Next
                        'Bishop
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 1, 1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, 1, -1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, -1, -1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                        RBQMoves(DiffASC, TempBoard, i, MoveFrom, MoveTo, -1, 1, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                    Case "k"
                        'king castling 
                        If TempNumOfMoves(Start + 12) = 0 Then
                            If TempNumOfMoves(Start + 8) = 0 Then
                                If TempBoard(ToCoordX(TempLocation(i)(0)) - 3, ToCoordY(TempLocation(i)(1))) = " " And TempBoard(ToCoordX(TempLocation(i)(0)) - 2, ToCoordY(TempLocation(i)(1))) = " " And TempBoard(ToCoordX(TempLocation(i)(0)) - 1, ToCoordY(TempLocation(i)(1))) = " " Then
                                    PossibleAdd(MoveFrom, MoveTo, -2, 0, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                End If
                            End If
                            If TempNumOfMoves(Start + 15) = 0 Then
                                If TempBoard(ToCoordX(TempLocation(i)(0)) + 2, ToCoordY(TempLocation(i)(1))) = " " And TempBoard(ToCoordX(TempLocation(i)(0)) + 1, ToCoordY(TempLocation(i)(1))) = " " Then
                                    PossibleAdd(MoveFrom, MoveTo, 2, 0, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                End If
                            End If
                        End If
                        'moving regularly 1 square
                        For x = -1 To 1
                            For y = -1 To 1
                                If Not (y = 0 And x = 0) Then
                                    If ToCoordX(TempLocation(i)(0)) + x >= 1 And ToCoordX(TempLocation(i)(0)) + x <= 8 And ToCoordY(TempLocation(i)(1)) + y >= 1 And ToCoordY(TempLocation(i)(1)) + y <= 8 Then
                                        'no piece is there
                                        If TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) <> " " Then
                                            'check if opponents piece is there
                                            For o = 0 To 5
                                                If TempBoard(ToCoordX(TempLocation(i)(0)) + x, ToCoordY(TempLocation(i)(1)) + y) = Chr(Asc(PieceSymbol(o)) - DiffASC) Then
                                                    PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                                End If
                                            Next
                                        Else
                                            PossibleAdd(MoveFrom, MoveTo, x, y, i, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                                        End If
                                    End If
                                End If
                            Next
                        Next
                End Select
            End If
        Next
        'adding everything to the arrays
        Array.Resize(PossibleMoveFrom, MoveFrom.Length)
        Array.Resize(PossibleMoveTo, MoveTo.Length)
        For i = 0 To MoveFrom.Length - 1
            PossibleMoveFrom(i) = MoveFrom(i)
            PossibleMoveTo(i) = MoveTo(i)
        Next
    End Sub
    'add a move from and move too int the the arrays
    Sub PossibleAdd(ByRef MoveFrom() As String, ByRef MoveTo() As String, ByVal ChangeX As Integer, ByVal ChangeY As Integer, ByVal TempI As Integer, ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByVal Player As Char, ByVal IgnTriedBefore As Boolean)
        'see if it is already in the array
        Dim Found As Boolean = False
        For i = 0 To MoveFrom.Length - 1
            If MoveFrom(i) = TempLocation(TempI) And MoveTo(i) = (Xaxis(ToCoordX(TempLocation(TempI)(0)) + ChangeX)) + Yaxis(ToCoordY(TempLocation(TempI)(1)) + ChangeY) Then
                Found = True
            End If
        Next
        'adding it if it is not already in the array
        If Found = False Then
            'see if it has been tried before
            Dim TriedBefore As Boolean = False
            If IgnTriedBefore = False Then
                For i = 0 To TriedMoveFrom.Length - 1
                    If TriedMoveFrom(i) = TempLocation(TempI) And TriedMoveTo(i) = (Xaxis(ToCoordX(TempLocation(TempI)(0)) + ChangeX)) + Yaxis(ToCoordY(TempLocation(TempI)(1)) + ChangeY) Then
                        'it has been found before
                        TriedBefore = True
                    End If
                Next
            End If
            'if it wasnt found then add it
            If TriedBefore = False Then
                Array.Resize(MoveFrom, MoveFrom.Length + 1)
                MoveFrom(MoveFrom.Length - 1) = TempLocation(TempI)
                Array.Resize(MoveTo, MoveTo.Length + 1)
                MoveTo(MoveTo.Length - 1) = (Xaxis(ToCoordX(TempLocation(TempI)(0)) + ChangeX)) + Yaxis(ToCoordY(TempLocation(TempI)(1)) + ChangeY)
            End If
        End If
    End Sub
    'check if a pawn can take
    Sub PossiblePawnTake(ByVal DiffASc As Integer, ByVal TempBoard(,) As Char, ByVal TempI As Integer, ByRef MoveFrom() As String, ByRef MoveTo() As String, ByVal x As Integer, ByVal Forward As Integer, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByVal Player As Char, ByVal IgnTriedBefore As Boolean)
        'Check if there is something diagonally infront of the pawn
        If ToCoordX(TempLocation(TempI)(0)) + x >= 1 And ToCoordX(TempLocation(TempI)(0)) + x <= 8 And ToCoordY(TempLocation(TempI)(1)) + Forward >= 1 And ToCoordY(TempLocation(TempI)(1)) + Forward <= 8 Then
            If TempBoard(ToCoordX(TempLocation(TempI)(0)) + x, ToCoordY(TempLocation(TempI)(1)) + Forward) <> " " Then
                'check if the thing diagonally is the opponets piece
                For o = 0 To 5
                    If TempBoard(ToCoordX(TempLocation(TempI)(0)) + x, ToCoordY(TempLocation(TempI)(1)) + Forward) = Chr(Asc(PieceSymbol(o)) - DiffASc) Then
                        PossibleAdd(MoveFrom, MoveTo, x, Forward, TempI, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                    End If
                Next
            End If
        End If
    End Sub
    'Check the Rook and Bishop moves
    Sub RBQMoves(ByVal DiffASc As Integer, ByVal TempBoard(,) As Char, ByVal TempI As Integer, ByRef MoveFrom() As String, ByRef MoveTo() As String, ByVal x As Integer, ByVal y As Integer, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByVal Player As Char, ByVal IgnTriedBefore As Boolean)
        'setting the variables
        Dim Length As Integer = 0
        Dim FirstTake As Boolean = False
        Dim Blockage As Boolean = False
        Dim Outside As Boolean = False
        Dim SecondTake As Boolean = False
        'looping until something prevents the piece from moving
        Do
            If FirstTake = True Then
                SecondTake = True
            End If
            'increase the length by 1 so that it checks one further
            Length += 1
            If ToCoordX(TempLocation(TempI)(0)) + (Length * x) <= 8 And ToCoordX(TempLocation(TempI)(0)) + (Length * x) >= 1 And ToCoordY(TempLocation(TempI)(1)) + (Length * y) <= 8 And ToCoordY(TempLocation(TempI)(1)) + (Length * y) >= 1 Then
                If TempBoard(ToCoordX(TempLocation(TempI)(0)) + (Length * x), ToCoordY(TempLocation(TempI)(1)) + (Length * y)) = " " Then
                    PossibleAdd(MoveFrom, MoveTo, x * Length, y * Length, TempI, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                Else
                    'check if it is the opponets
                    For o = 0 To 5
                        If TempBoard(ToCoordX(TempLocation(TempI)(0)) + (Length * x), ToCoordY(TempLocation(TempI)(1)) + (Length * y)) = Chr(Asc(PieceSymbol(o)) - DiffASc) Then
                            PossibleAdd(MoveFrom, MoveTo, x * Length, y * Length, TempI, TempBoard, TempLocation, TempNumOfMoves, Player, IgnTriedBefore)
                            'the opponets peice postion
                            FirstTake = True
                        Else
                            'the currnt playesr peice is blocking the way
                            Blockage = True
                        End If
                    Next
                End If
            Else
                'off the board
                Outside = True
            End If
        Loop Until SecondTake = True Or Blockage = True Or Outside = True
    End Sub
    'Looks ahead in the board by simulating moves and analysing them to find the best one
    Sub LookAhead(ByVal TempBoard(,) As Char, ByVal TempLocation() As String, ByVal TempNumOfMoves() As Integer, ByVal Moves As Integer, ByRef OriginalMoveFrom As String, ByRef OriginalMoveTo As String, ByRef LowestArrFrom() As String, ByRef LowestArrTo() As String, ByRef LowestArrScore() As Integer, ByVal DepthCounter As Integer, ByVal TempPieces() As String, ByVal CallInter As Boolean)
        'figure out the opponets number
        OP = "1"
        If CurrentPlayer = "1" Then
            OP = "2"
        End If
        'dim the possible arrays
        Dim CompPossibleMoveFrom(0) As String
        Dim CompPossibleMoveTo(0) As String
        Dim HumPossibleMoveFrom(0) As String
        Dim HumPossibleMoveTo(0) As String
        If DepthCounter = 0 And Moves = 0 Then
            'take into account the moves to avoid and get the possible moves
            Possible(CurrentPlayer, TempBoard, TempLocation, TempNumOfMoves, CompPossibleMoveFrom, CompPossibleMoveTo, False)
        Else
            'dont take into account moves to avoid and get the possible moves
            Possible(CurrentPlayer, TempBoard, TempLocation, TempNumOfMoves, CompPossibleMoveFrom, CompPossibleMoveTo, True)
        End If
        If CompPossibleMoveFrom.Length = 0 Then
            Console.WriteLine("StaleMate2")
            'no legal moves
        Else
            'go through the the whole possible moves
            For i = 1 To CompPossibleMoveFrom.Length - 1
                If Moves = 0 And DepthCounter = 0 Then
                    'collect the original moves
                    OriginalMoveFrom = CompPossibleMoveFrom(i)
                    OriginalMoveTo = CompPossibleMoveTo(i)
                End If
                'set the lowest really high
                Dim Lowest As Integer = 10 * KingCoefficent
                'copy the arrays to come back to
                Dim BoardAfterCompMove(8, 8) As Char
                CopyBoards(TempBoard, BoardAfterCompMove)
                Dim LocationAfterCompMove(0) As String
                CopyStrArr(TempLocation, LocationAfterCompMove)
                Dim NumOfMoveAfterCompMove(0) As Integer
                CopyIntArr(TempNumOfMoves, NumOfMoveAfterCompMove)
                Dim PieceComp(0) As String
                CopyStrArr(TempPieces, PieceComp)
                'simulate the moves
                SimMove(BoardAfterCompMove, LocationAfterCompMove, NumOfMoveAfterCompMove, CompPossibleMoveFrom(i), CompPossibleMoveTo(i), PieceComp)
                'calculate the possible moves of the oppostion
                Possible(OP, BoardAfterCompMove, LocationAfterCompMove, NumOfMoveAfterCompMove, HumPossibleMoveFrom, HumPossibleMoveTo, True)
                If HumPossibleMoveFrom.Length = 0 Then
                    Console.WriteLine("StaleMate1")
                    'no legal moves
                Else
                    'go through the opponets possible moves
                    For o = 1 To HumPossibleMoveFrom.Length - 1
                        ' copy the arrays to come back too
                        Dim BoardAfterHum(8, 8) As Char
                        CopyBoards(BoardAfterCompMove, BoardAfterHum)
                        Dim LocationAfterHumMove(0) As String
                        CopyStrArr(LocationAfterCompMove, LocationAfterHumMove)
                        Dim NumOfMoveAfterHumMove(0) As Integer
                        CopyIntArr(NumOfMoveAfterCompMove, NumOfMoveAfterHumMove)
                        Dim PieceHum(0) As String
                        CopyStrArr(PieceComp, PieceHum)
                        'simulate the moves
                        SimMove(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, HumPossibleMoveFrom(o), HumPossibleMoveTo(o), PieceHum)
                        If Moves = Depth Then
                            If CurrentMoveDepth = 0 Then
                                'evaluate the board
                                Dim CurrentBoardScore As Integer = EvaluateScore(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, PieceHum)
                                Dim Found As Boolean = False
                                'search through the lowest score array to find the original move scores
                                For l = 0 To LowestArrScore.GetUpperBound(0)
                                    If LowestArrFrom(l) = OriginalMoveFrom And LowestArrTo(l) = OriginalMoveTo Then
                                        Found = True
                                        If CurrentBoardScore < LowestArrScore(l) Then
                                            LowestArrScore(l) = CurrentBoardScore
                                            'change the lowest score
                                        End If
                                    End If
                                Next
                                Combinations += 1
                                'add the original moves and score to the arrays
                                If Found = False Then
                                    Array.Resize(LowestArrFrom, LowestArrFrom.Length + 1)
                                    LowestArrFrom(LowestArrFrom.Length - 1) = OriginalMoveFrom
                                    Array.Resize(LowestArrTo, LowestArrTo.Length + 1)
                                    LowestArrTo(LowestArrTo.Length - 1) = OriginalMoveTo
                                    Array.Resize(LowestArrScore, LowestArrScore.Length + 1)
                                    LowestArrScore(LowestArrScore.Length - 1) = CurrentBoardScore
                                End If
                            Else
                                'it has gone to the required depth before re-evaluating
                                If DepthCounter = CurrentMoveDepth Then
                                    If CallInter = False Then
                                        'evaluate the board
                                        Dim CurrentBoardScore As Integer = EvaluateScore(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, PieceHum)
                                        Dim Found As Boolean = False
                                        For l = 0 To LowestArrScore.GetUpperBound(0)
                                            If LowestArrFrom(l) = OriginalMoveFrom And LowestArrTo(l) = OriginalMoveTo Then
                                                Found = True
                                                If CurrentBoardScore < LowestArrScore(l) Then
                                                    LowestArrScore(l) = CurrentBoardScore
                                                    'changed the lowest array
                                                End If
                                            End If
                                        Next
                                        Combinations += 1
                                        'adds the original moves and the lowest score to the arrays
                                        If Found = False Then
                                            Array.Resize(LowestArrFrom, LowestArrFrom.Length + 1)
                                            LowestArrFrom(LowestArrFrom.Length - 1) = OriginalMoveFrom
                                            Array.Resize(LowestArrTo, LowestArrTo.Length + 1)
                                            LowestArrTo(LowestArrTo.Length - 1) = OriginalMoveTo
                                            Array.Resize(LowestArrScore, LowestArrScore.Length + 1)
                                            LowestArrScore(LowestArrScore.Length - 1) = CurrentBoardScore
                                        End If
                                    Else
                                        'gets the interesting value
                                        Dim Interesting As Integer = 0
                                        Interesting += PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                        Interesting += TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                        Interesting += TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                        Interesting -= PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                        Interesting -= TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                        Interesting -= TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                        'adds the interesting to the array that is used to calculate the interesting coefficient
                                        Array.Resize(Interestings, Interestings.Length + 1)
                                        Interestings(Interestings.Length - 1) = Interesting
                                    End If

                                Else
                                    'calculated the interesting value of the position
                                    Dim Interesting As Integer = 0
                                    Interesting += PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                    Interesting += TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                    Interesting += TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                    Interesting -= PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                    Interesting -= TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                    Interesting -= TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                    'adds the interesting to the array that is used to calculate the interesting coefficient if the callInter is true
                                    If CallInter = True Then
                                        Array.Resize(Interestings, Interestings.Length + 1)
                                        Interestings(Interestings.Length - 1) = Interesting
                                    Else
                                        'see if the interesting value is great enough to look deeper
                                        If Interesting > InterestingCoefficient Then
                                            'look deeper with the recursion
                                            LookAhead(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, 0, OriginalMoveFrom, OriginalMoveTo, LowestArrFrom, LowestArrTo, LowestArrScore, DepthCounter + 1, TempPieces, False)
                                        Else
                                            'if it isnt then evaluate the board
                                            Dim CurrentBoardScore As Integer = EvaluateScore(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, PieceHum)
                                            Dim Found As Boolean = False
                                            'update the arrays if required
                                            For l = 0 To LowestArrScore.GetUpperBound(0)
                                                If LowestArrFrom(l) = OriginalMoveFrom And LowestArrTo(l) = OriginalMoveTo Then
                                                    Found = True
                                                    If CurrentBoardScore < LowestArrScore(l) Then
                                                        LowestArrScore(l) = CurrentBoardScore
                                                        'change the lowest
                                                    End If
                                                End If
                                            Next
                                            Combinations += 1
                                            'add the original moves and lowest value into the arrays
                                            If Found = False Then
                                                Array.Resize(LowestArrFrom, LowestArrFrom.Length + 1)
                                                LowestArrFrom(LowestArrFrom.Length - 1) = OriginalMoveFrom
                                                Array.Resize(LowestArrTo, LowestArrTo.Length + 1)
                                                LowestArrTo(LowestArrTo.Length - 1) = OriginalMoveTo
                                                Array.Resize(LowestArrScore, LowestArrScore.Length + 1)
                                                LowestArrScore(LowestArrScore.Length - 1) = CurrentBoardScore
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            If DepthCounter = CurrentMoveDepth Then
                                Dim CurrentBoardScore As Integer = EvaluateScore(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, PieceHum)
                                Dim Found As Boolean = False
                                'update the arrays if required
                                For l = 0 To LowestArrScore.GetUpperBound(0)
                                    If LowestArrFrom(l) = OriginalMoveFrom And LowestArrTo(l) = OriginalMoveTo Then
                                        Found = True
                                        If CurrentBoardScore < LowestArrScore(l) Then
                                            LowestArrScore(l) = CurrentBoardScore
                                            'change lowest value 
                                        End If
                                    End If
                                Next
                                Combinations += 1
                                'add the original move to the array and the score
                                If Found = False Then
                                    Array.Resize(LowestArrFrom, LowestArrFrom.Length + 1)
                                    LowestArrFrom(LowestArrFrom.Length - 1) = OriginalMoveFrom
                                    Array.Resize(LowestArrTo, LowestArrTo.Length + 1)
                                    LowestArrTo(LowestArrTo.Length - 1) = OriginalMoveTo
                                    Array.Resize(LowestArrScore, LowestArrScore.Length + 1)
                                    LowestArrScore(LowestArrScore.Length - 1) = CurrentBoardScore
                                End If
                            Else
                                'calculate the interesting value
                                Dim Interesting As Integer = 0
                                Interesting += PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                Interesting += TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                Interesting += TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, CurrentPlayer)
                                Interesting -= PPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                Interesting -= TPV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                Interesting -= TUV(BoardAfterHum, NumOfMoveAfterHumMove, LocationAfterHumMove, PieceHum, OP)
                                If CallInter = True Then
                                    Array.Resize(Interestings, Interestings.Length + 1)
                                    Interestings(Interestings.Length - 1) = Interesting
                                Else
                                    If Interesting > InterestingCoefficient Then
                                        'recursively call the lookahead with increased depth
                                        LookAhead(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, 0, OriginalMoveFrom, OriginalMoveTo, LowestArrFrom, LowestArrTo, LowestArrScore, DepthCounter + 1, TempPieces, False)
                                    Else
                                        'evaluate the board
                                        Dim CurrentBoardScore As Integer = EvaluateScore(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, PieceHum)
                                        Dim Found As Boolean = False
                                        'update the arrays
                                        For l = 0 To LowestArrScore.GetUpperBound(0)
                                            If LowestArrFrom(l) = OriginalMoveFrom And LowestArrTo(l) = OriginalMoveTo Then
                                                Found = True
                                                If CurrentBoardScore < LowestArrScore(l) Then
                                                    LowestArrScore(l) = CurrentBoardScore
                                                    'change lowest
                                                End If
                                            End If
                                        Next
                                        Combinations += 1
                                        'add the original moves nad the lowest array
                                        If Found = False Then
                                            Array.Resize(LowestArrFrom, LowestArrFrom.Length + 1)
                                            LowestArrFrom(LowestArrFrom.Length - 1) = OriginalMoveFrom
                                            Array.Resize(LowestArrTo, LowestArrTo.Length + 1)
                                            LowestArrTo(LowestArrTo.Length - 1) = OriginalMoveTo
                                            Array.Resize(LowestArrScore, LowestArrScore.Length + 1)
                                            LowestArrScore(LowestArrScore.Length - 1) = CurrentBoardScore
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            'call look ahead again because the minimum depth was not reached
                            LookAhead(BoardAfterHum, LocationAfterHumMove, NumOfMoveAfterHumMove, Moves + 1, OriginalMoveFrom, OriginalMoveTo, LowestArrFrom, LowestArrTo, LowestArrScore, DepthCounter, TempPieces, CallInter)
                        End If
                    Next
                End If
            Next
        End If
    End Sub
    'checks in a file to find a move to make from a board state
    Sub CheckFile(ByRef BestMoveFrom As String, ByRef BestMoveTo As String, ByRef Found As Boolean, ByVal FileName As String)
        Dim BoardState As String = BoardLine(Board)
        'reads all the lines into an array
        Dim Lines() As String = File.ReadAllLines(MainFilePath + "Chess\Computer\" + FileName + ".txt")
        For i = 0 To Lines.Length - 1
            'splits the lines to get the boardstate as {0}
            Dim SplitLine() As String = Lines(i).Split(",")
            If BoardState = SplitLine(0) Then
                Found = True
                'add up to get the total weight
                Dim TotalWeight As Integer
                For o = 1 To SplitLine.Length - 1
                    Dim SplitSplitLine() As String = SplitLine(o).Split(":")
                    TotalWeight += Convert.ToInt16(SplitSplitLine(0))
                Next
                'picks a weight at random
                Randomize()
                Dim SplitLineVal As Integer
                Dim Rand As Integer = Math.Floor(TotalWeight * Rnd()) + 1
                'goes through all the weights
                For o = 1 To SplitLine.Length - 1
                    Dim SplitSplitLine() As String = SplitLine(o).Split(":")
                    'subtract the weighting
                    Rand -= Convert.ToInt16(SplitSplitLine(0))
                    If Rand <= 0 Then
                        'picking the move
                        SplitLineVal = o
                        Exit For
                    End If
                Next
                'finalising picking them move
                Dim MoveFromTo As String = SplitLine(SplitLineVal)
                Dim SplitMoveFromTo() As String = MoveFromTo.Split(":")
                BestMoveFrom = SplitMoveFromTo(1)
                BestMoveTo = SplitMoveFromTo(2)
            End If
        Next
    End Sub
    'the computes main pard of the move
    Sub ComputerMove(ByVal Learning As Boolean)
        Dim Found As Boolean = False
        Dim BestMoveFromFile As String = ""
        Dim BestMoveToFile As String = ""
        If TotalMoves < 8 Then
            'check the OPening files
            If CurrentPlayer = "1" Then
                'check whiteOPenings
                CheckFile(BestMoveFromFile, BestMoveToFile, Found, "WhiteOpenings")
            Else
                'check blackOPenings
                CheckFile(BestMoveFromFile, BestMoveToFile, Found, "BlackOpenings")
            End If
        End If
        'if the boardstate wasnt found in the OPenings
        If Found = False Then
            If UsingDataBase = False Then
                CurrentMoveDepth = MaxDepth
                Dim AgainMove As Boolean = False
                Do
                    Try
                        MostOfTurn(Learning)
                        CurrentMoveDepth += 1
                    Catch ex As Exception
                        If CurrentMoveDepth > 0 Then
                            CurrentMoveDepth -= 1
                        Else
                            Do
                                Dim MoveFrom(0) As String
                                Dim MoveTo(0) As String
                                Possible(CurrentPlayer, Board, Location, NumOfMoves, MoveFrom, MoveTo, True)
                                Randomize()
                                Dim Num As Integer = (MoveFrom.Length - 2) * Rnd() + 1
                                Move(CurrentPlayer, MoveFrom(Num), MoveTo(Num))
                            Loop Until Valid = True
                        End If
                    End Try
                Loop Until AgainMove = False
            Else
                'check the database
                Dim FoundData As Boolean = False
                Dim BestMoveFromData As String = ""
                Dim BestMoveToData As String = ""
                If CurrentPlayer = "1" Then
                    'check the white database
                    CheckFile(BestMoveFromData, BestMoveToData, FoundData, "WhiteDataBase")
                Else
                    'check the black database
                    CheckFile(BestMoveFromData, BestMoveToData, FoundData, "BlackDataBase")
                End If
                'try working out the best move if the data wasnt found
                If FoundData = False Then
                    CurrentMoveDepth = MaxDepth
                    Dim AgainMove As Boolean = False
                    Do
                        'try to work out the move at the current depth
                        Try
                            MostOfTurn(Learning)
                            CurrentMoveDepth += 1
                        Catch ex As Exception
                            'if the current depth is too much then decrese the move and loop around and try again
                            If CurrentMoveDepth > 0 Then
                                CurrentMoveDepth -= 1
                            Else
                                'unless the current depth is already 0
                                Do
                                    'get the possible moves
                                    Dim MoveFrom(0) As String
                                    Dim MoveTo(0) As String
                                    Possible(CurrentPlayer, Board, Location, NumOfMoves, MoveFrom, MoveTo, True)
                                    Randomize()
                                    'Randomly pick a possible move it it cannot be handled
                                    Dim Num As Integer = (MoveFrom.Length - 1) * Rnd() + 1
                                    'make the random move
                                    Move(CurrentPlayer, MoveFrom(Num), MoveTo(Num))
                                Loop Until Valid = True
                            End If
                        End Try
                    Loop Until AgainMove = False
                Else
                    'make the move of whatever was decided in the file
                    If Learning = False Then
                        If GameMode <> "CVC" Then
                            'announce the move umless it is CVC
                            Console.WriteLine(BestMoveFromData + " to " + BestMoveToData)
                            Console.ReadLine()
                        End If
                        'make the moves
                        Move(CurrentPlayer, BestMoveFromData, BestMoveToData)
                        'update the last move variables
                        LastMoveFrom = BestMoveFromData
                        LastMoveTo = BestMoveToData
                    Else
                        'set the learning vairable so that they can be compared
                        LearningMoveFrom = BestMoveFromData
                        LearningMoveTo = BestMoveToData
                    End If
                End If
            End If
        Else
            If Learning = False Then
                'make the move of whatever was decided in the file
                If GameMode <> "CVC" Then
                    'anncounce the move
                    Console.WriteLine(BestMoveFromFile + " to " + BestMoveToFile)
                    Console.ReadLine()
                End If
                'make the move
                Move(CurrentPlayer, BestMoveFromFile, BestMoveToFile)
                'update the last move made
                LastMoveFrom = BestMoveFromFile
                LastMoveTo = BestMoveToFile
            Else
                'learning data to be compared later
                LearningMoveFrom = BestMoveFromFile
                LearningMoveTo = BestMoveToFile
            End If
        End If
    End Sub
    'most of the computers turn
    Sub MostOfTurn(ByVal Learning As Boolean)
        'copy all the arrays
        Dim TempBoardINT(8, 8) As Char
        CopyBoards(Board, TempBoardINT)
        Dim TempLocationINT(0) As String
        CopyStrArr(Location, TempLocationINT)
        Dim TempNumOfMoveINT(0) As Integer
        CopyIntArr(NumOfMoves, TempNumOfMoveINT)
        Dim TempPiecesINT(0) As String
        CopyStrArr(Piece, TempPiecesINT)
        'reset everything
        Combinations = 0
        Dim OriginalMoveFromINT As String = ""
        Dim OriginalMoveToINT As String = ""
        Dim LowestArrFromINT(0) As String
        Dim LowestArrToINT(0) As String
        Dim LowestArrScoreINT(0) As Integer
        If CurrentMoveDepth > 0 Then
            'call lookahead to get all the data interesting data so that the interesting coefficient can be calculated
            LookAhead(TempBoardINT, TempLocationINT, TempNumOfMoveINT, 0, OriginalMoveFromINT, OriginalMoveToINT, LowestArrFromINT, LowestArrToINT, LowestArrScoreINT, 0, TempPiecesINT, True)
            If Interestings.Length > 1 Then
                'calculate the interesting value
                InterestingCoefficient = UpperPercentage()
                If InterestingCoefficient < 0 Then
                    InterestingCoefficient = 0
                End If
            End If
        End If
        'start calculating if it is near the ned of the game
        Dim PieceValue1 As Integer
        Dim PieceValue2 As Integer
        'work ut what player it is
        OP = "1"
        If CurrentPlayer = "1" Then
            OP = "2"
        End If
        'get the corrrect coefficeients
        Dim PawnCoefficent As Integer
        Dim RookCoefficent As Integer
        Dim KnightCoefficent As Integer
        Dim BishopCoefficent As Integer
        Dim QueenCoefficent As Integer
        If CurrentPlayer = "1" Then
            PawnCoefficent = Pawn1Coefficent
            RookCoefficent = Rook1Coefficent
            KnightCoefficent = Knight1Coefficent
            BishopCoefficent = Bishop1Coefficent
            QueenCoefficent = Queen1Coefficent
        Else
            PawnCoefficent = Pawn2Coefficent
            RookCoefficent = Rook2Coefficent
            KnightCoefficent = Knight2Coefficent
            BishopCoefficent = Bishop2Coefficent
            QueenCoefficent = Queen2Coefficent
        End If
        'work out the value of all the pieces of both sides
        For y = 1 To 8
            For x = 1 To 8
                If Board(y, x) <> " " Then
                    Select Case (Board(y, x))
                        Case "p"
                            PieceValue1 += PawnCoefficent
                        Case "r"
                            PieceValue1 += RookCoefficent
                        Case "n"
                            PieceValue1 += KnightCoefficent
                        Case "b"
                            PieceValue1 += BishopCoefficent
                        Case "q"
                            PieceValue1 += QueenCoefficent
                        Case "P"
                            PieceValue2 += PawnCoefficent
                        Case "R"
                            PieceValue2 += RookCoefficent
                        Case "N"
                            PieceValue2 += KnightCoefficent
                        Case "B"
                            PieceValue2 += BishopCoefficent
                        Case "Q"
                            PieceValue2 += QueenCoefficent
                    End Select
                End If
            Next
        Next
        'enable it so that the king squares are taken into account
        If PieceValue1 < 1200 Or PieceValue2 < 1200 Then
            NearEndGame = True
        Else
            Do
                'copy all the arrays
                Dim TempBoard(8, 8) As Char
                CopyBoards(Board, TempBoard)
                Dim TempLocation(0) As String
                CopyStrArr(Location, TempLocation)
                Dim TempNumOfMove(0) As Integer
                CopyIntArr(NumOfMoves, TempNumOfMove)
                Dim TempPieces(0) As String
                CopyStrArr(Piece, TempPieces)
                Dim Score As Integer = 0
                'reset all the variables
                Combinations = 0
                Dim OriginalMoveFrom As String = ""
                Dim OriginalMoveTo As String = ""
                Dim LowestArrFrom(0) As String
                Dim LowestArrTo(0) As String
                Dim LowestArrScore(0) As Integer
                'look ahead
                LookAhead(TempBoard, TempLocation, TempNumOfMove, 0, OriginalMoveFrom, OriginalMoveTo, LowestArrFrom, LowestArrTo, LowestArrScore, 0, TempPieces, False)
                Dim BestMoveFrom(0) As String
                Dim BestMoveTo(0) As String
                'set the highest really low
                'this is the max part of the minimax
                Dim Highest As Integer = -10 * KingCoefficent
                For i = 1 To LowestArrFrom.Length - 1
                    If LowestArrScore(i) + 10 * KingCoefficent >= Highest Then
                        If LowestArrScore(i) + 10 * KingCoefficent > Highest Then
                            'change the new highest
                            Highest = LowestArrScore(i) + 10 * KingCoefficent
                            Array.Resize(BestMoveFrom, 0)
                            Array.Resize(BestMoveTo, 0)
                        End If
                        'add the same value one
                        Array.Resize(BestMoveFrom, BestMoveFrom.Length + 1)
                        BestMoveFrom(BestMoveFrom.Length - 1) = LowestArrFrom(i)
                        Array.Resize(BestMoveTo, BestMoveTo.Length + 1)
                        BestMoveTo(BestMoveTo.Length - 1) = LowestArrTo(i)
                    End If
                Next
                Dim Number As Integer = CInt(Math.Floor((BestMoveFrom.Length - 1) * Rnd()))
                If Learning = False Then
                    If GameMode <> "CVC" Then
                        'announce the move
                        Console.WriteLine(BestMoveFrom(Number) + " to " + BestMoveTo(Number))
                        Console.ReadLine()
                    End If
                    'make the move
                    Move(CurrentPlayer, BestMoveFrom(Number), BestMoveTo(Number))
                    'update the last move
                    LastMoveFrom = BestMoveFrom(Number)
                    LastMoveTo = BestMoveTo(Number)
                Else
                    'record the learning value if it is learning
                    LearningMoveFrom = BestMoveFrom(Number)
                    LearningMoveTo = BestMoveTo(Number)
                End If
            Loop Until Valid = True
        End If
    End Sub
    'og learning Sub
    Sub Learn()
        Dim Now As Integer
        'setting the stop date and time
        Dim StopDate As String = "08/10/2018"
        Dim StopTime As String = "08:00:00"
        Console.WriteLine("What date would you like to stop")
        Console.WriteLine("Format: DD/MM/YYYY")
        StopDate = Console.ReadLine()
        Console.WriteLine("What time would you like to stop")
        Console.WriteLine("Format: HH:MM:SS")
        StopTime = Console.ReadLine()
        Dim StopDateArr() As String = StopDate.Split("/")
        Dim StopTimeArr() As String = StopTime.Split(":")
        Dim StopDateTime As DateTime = New DateTime(Convert.ToInt16(StopDateArr(2)), Convert.ToInt16(StopDateArr(1)), Convert.ToInt16(StopDateArr(0)), Convert.ToInt16(StopTimeArr(0)), Convert.ToInt16(StopTimeArr(1)), Convert.ToInt16(StopTimeArr(2)))
        Dim StopDateTimeInt As Integer = (StopDateTime - StartDate).TotalSeconds
        'see if the file of iterations exists
        If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\Learning\Iterations.txt") = True Then
            'create it if it doesnt
            System.IO.File.Create(MainFilePath + "Chess\Computer\Learning\Iterations.txt").Dispose()
            Dim objWriter As New System.IO.StreamWriter(MainFilePath + "Chess\Computer\Learning\Iterations.txt")
            'write 0 in the file if it was empty
            objWriter.WriteLine("0")
            objWriter.Close()
        End If
        'read the first line iteretion file
        Dim Iterations As Integer = Convert.ToInt16(ReadLineWithNumberFrom(MainFilePath + "Chess\Computer\Learning\Iterations.txt", 1))
        Do
            'reset
            LearningScore = 0
            Now = (DateTime.Now - StartDate).TotalSeconds
            Iterations += 1
            If Iterations = 1 Then
                Using writer As New StreamWriter(MainFilePath + "Chess\Computer\Learning\Results.csv", True)
                    writer.WriteLine("Iteration,Pawn,Rook,Knight,Bishop,Queen,Move,Score,Time")
                End Using
                'create a set of random coefficeints
                Randomize()
                Pawn2Coefficent = CInt(Math.Floor((40) * Rnd())) + 80
                Rook2Coefficent = CInt(Math.Floor((200) * Rnd())) + 400
                Knight2Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Bishop2Coefficent = CInt(Math.Floor((120) * Rnd())) + 240
                Queen2Coefficent = CInt(Math.Floor((360) * Rnd())) + 720
                Move2NumberCoefficent = (CInt(Math.Floor((40) * Rnd())) + 80) / 100
                ProtectedPieces2Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
                ThreatenedPieces2Coefficent = (CInt(Math.Floor((4) * Rnd())) + 8) / 100
            End If
            'loop through all the test data
            For i = 1 To 16
                'load in the game
                Load(MainFilePath + "Chess\Computer\Learning\TestData\" + Convert.ToString(i) + ".txt")
                'get the actual move made
                Dim ActMoveFrom As String = LoadingReason(0) + LoadingReason(1)
                Dim ActMoveTo As String = LoadingReason(3) + LoadingReason(4)
                CurrentPlayer = "2"
                'display the board from white perspective
                Display("1")
                'run the computer move
                ComputerMove(True)
                'compare the move the computer made with the actual one
                If LearningMoveFrom = ActMoveFrom And LearningMoveTo = ActMoveTo Then
                    'they were the same
                    LearningScore += 1
                End If
            Next
            'write the results to the file
            Dim LearningDateNow3 As String = Replace(Convert.ToString(DateTime.Now), "#", "")
            Dim LearningDateNow2() As String = Split(DateNow3, " ")
            Dim LearningDateNow As String = Replace(DateNow2(0), "/", "-")
            Using writer As New StreamWriter(MainFilePath + "Chess\Computer\Learning\Results.csv", True)
                writer.WriteLine(Convert.ToString(Iterations) + "," + Convert.ToString(Pawn2Coefficent) + "," + Convert.ToString(Rook2Coefficent) + "," + Convert.ToString(Knight2Coefficent) + "," + Convert.ToString(Bishop2Coefficent) + "," + Convert.ToString(Queen2Coefficent) + "," + Convert.ToString(Move2NumberCoefficent) + "," + Convert.ToString(ThreatenedPieces2Coefficent) + "," + Convert.ToString(ProtectedPieces2Coefficent) + "," + Convert.ToString(LearningScore) + "," + LearningDateNow + " " + TimestampNow())
            End Using
            'stop if it is past the stoptime
        Loop Until Now >= StopDateTimeInt
    End Sub
    'review mode
    Sub Review(ByVal GameFile As String)
        'read all the lines of the file in compressed moves to get the move number 
        Dim Selected As Integer = File.ReadAllLines(MainFilePath + "Chess\" + GameMode + "\Games\" + GameFile + "\CompressedMove.txt").Length
        Dim ActualTotalMoves = Selected
        Dim PlayerNumber As Char = "2"
        Console.Clear()
        Dim Entry As String = "RightArrow"
        Dim k As ConsoleKeyInfo
        Do
            Select Case (Entry)
                Case "LeftArrow"
                    If Not Selected = 0 Then
                        Selected = Selected - 1
                        'back on the review
                    End If
                Case "RightArrow"
                    If Not Selected = ActualTotalMoves Then
                        Selected = Selected + 1
                        'forward on the review
                    End If
            End Select
            'reload the game
            Do
                Console.Clear()
                If Selected = 0 Then
                    Load(MainFilePath + "Chess\Computer\New.txt")
                    ReviewMoveFrom = "NA"
                    ReviewMoveTo = "NA"
                Else
                    Dim CorH As String = "Human"
                    If GameMode = "CVC" Then
                        CorH = "Computer"
                        PlayerNumber = "1"
                        If Selected Mod 2 = 0 Then
                            PlayerNumber = "2"
                        End If
                    ElseIf GameMode = "HVH" Then
                        CorH = "Human"
                        PlayerNumber = "1"
                        If Selected Mod 2 = 0 Then
                            PlayerNumber = "2"
                        End If
                    Else
                        PlayerNumber = "1"
                        If Selected Mod 2 = 0 Then
                            CorH = "Computer"
                            PlayerNumber = "2"
                        End If
                    End If
                    'loading the gamefile in the move requested
                    If GameMode = "CVH" Then
                        Load(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\Moves\" + Convert.ToString(Selected) + CorH + ".txt")
                    Else
                        If Selected = 1 Then
                            PlayerNumber = "1"
                        End If
                        Load(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\Moves\" + Convert.ToString(Selected) + CorH + PlayerNumber + ".txt")
                    End If
                End If
                'displays the board
                DisplayReview()
                k = Console.ReadKey(True)
                Entry = k.Key.ToString
            Loop Until Entry = "LeftArrow" Or Entry = "RightArrow" Or Entry = "E"
        Loop Until Entry = "E"
        'load the board back of the game you are playing
        Load(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\CurrentBoard.txt")
    End Sub
    'protected peices value
    Function PPV(ByVal TempBoard(,) As Char, ByVal TempNumMoves() As Integer, ByVal TempLocation() As String, ByVal TempPieces() As String, ByVal Player As Char) As Integer
        Dim Value As Integer
        'get the possible moves
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        Possible(Player, TempBoard, TempLocation, TempNumMoves, MoveFrom, MoveTo, True)
        'gets the coefficients
        Dim PawnCoefficent As Integer
        Dim RookCoefficent As Integer
        Dim KnightCoefficent As Integer
        Dim BishopCoefficent As Integer
        Dim QueenCoefficent As Integer
        If CurrentPlayer = "1" Then
            PawnCoefficent = Pawn1Coefficent
            RookCoefficent = Rook1Coefficent
            KnightCoefficent = Knight1Coefficent
            BishopCoefficent = Bishop1Coefficent
            QueenCoefficent = Queen1Coefficent
        Else
            PawnCoefficent = Pawn2Coefficent
            RookCoefficent = Rook2Coefficent
            KnightCoefficent = Knight2Coefficent
            BishopCoefficent = Bishop2Coefficent
            QueenCoefficent = Queen2Coefficent
        End If
        'get the correct player
        Dim Start As Integer = 1
        If Player = "2" Then
            Start = 17
        End If
        'sort through against the piece location and the possible moves of players so that when they overlap the piece is protected
        Dim AP(0) As String
        For i = 1 To MoveFrom.Length - 1
            For o = Start To Start + 15
                If MoveFrom(i) = TempLocation(o) Then
                    Dim Found As Boolean = False
                    For p = 1 To AP.Length - 1
                        If TempLocation(o) = AP(p) Then
                            Found = True
                        End If
                    Next
                    If Found = False Then
                        'count up the value
                        'not counting a piece twice if it is protected twice
                        Array.Resize(AP, AP.Length + 1)
                        AP(AP.Length - 1) = TempLocation(o)
                        Select Case LCase(TempPieces(o)(3))
                            Case "p"
                                Value += PawnCoefficent
                            Case "r"
                                Value += RookCoefficent
                            Case "n"
                                Value += KnightCoefficent
                            Case "b"
                                Value += BishopCoefficent
                            Case "q"
                                Value += QueenCoefficent
                        End Select
                    End If
                End If
            Next
        Next
        Return Value
    End Function
    'threatened piece value
    Function TPV(ByVal TempBoard(,) As Char, ByVal TempNumMoves() As Integer, ByVal TempLocation() As String, ByVal TempPieces() As String, ByVal Player As Char) As Integer
        Dim Value As Integer
        'possible moves for player
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        Possible(Player, TempBoard, TempLocation, TempNumMoves, MoveFrom, MoveTo, True)
        'get the correct coefficients
        Dim PawnCoefficent As Integer
        Dim RookCoefficent As Integer
        Dim KnightCoefficent As Integer
        Dim BishopCoefficent As Integer
        Dim QueenCoefficent As Integer
        Dim MoveNumberCoefficent As Double
        If CurrentPlayer = "1" Then
            PawnCoefficent = Pawn1Coefficent
            RookCoefficent = Rook1Coefficent
            KnightCoefficent = Knight1Coefficent
            BishopCoefficent = Bishop1Coefficent
            QueenCoefficent = Queen1Coefficent
            MoveNumberCoefficent = Move1NumberCoefficent
        Else
            PawnCoefficent = Pawn2Coefficent
            RookCoefficent = Rook2Coefficent
            KnightCoefficent = Knight2Coefficent
            BishopCoefficent = Bishop2Coefficent
            QueenCoefficent = Queen2Coefficent
            MoveNumberCoefficent = Move2NumberCoefficent
        End If
        'get hte correct variables
        Dim Start As Integer = 1
        If Player = "1" Then
            Start = 17
        End If
        'compare the location of the opponets peices and the player s possible moves
        'if there is overlap the player is threateneing the opponents piece
        Dim AP(0) As String
        For i = 1 To MoveTo.Length - 1
            For o = Start To Start + 15
                If MoveTo(i) = TempLocation(o) Then
                    Dim Found As Boolean = False
                    For p = 1 To AP.Length - 1
                        If TempLocation(o) = AP(p) Then
                            Found = True
                        End If
                    Next
                    If Found = False Then
                        'count up the value
                        Array.Resize(AP, AP.Length + 1)
                        AP(AP.Length - 1) = TempLocation(o)
                        Select Case LCase(TempPieces(o)(3))
                            Case "p"
                                Value += PawnCoefficent
                            Case "r"
                                Value += RookCoefficent
                            Case "n"
                                Value += KnightCoefficent
                            Case "b"
                                Value += BishopCoefficent
                            Case "q"
                                Value += QueenCoefficent
                        End Select
                    End If
                End If
            Next
        Next
        Return Value
    End Function
    'threatened uprotected value essentially a combination 
    Function TUV(ByVal TempBoard(,) As Char, ByVal TempNumMoves() As Integer, ByVal TempLocation() As String, ByVal TempPieces() As String, ByVal Player As Char) As Integer
        Dim Value As Integer = 0
        'possible value
        Dim MoveFrom(0) As String
        Dim MoveTo(0) As String
        Possible(Player, TempBoard, TempLocation, TempNumMoves, MoveFrom, MoveTo, True)
        'get the coefficients
        Dim PawnCoefficent As Integer
        Dim RookCoefficent As Integer
        Dim KnightCoefficent As Integer
        Dim BishopCoefficent As Integer
        Dim QueenCoefficent As Integer
        Dim MoveNumberCoefficent As Double
        If CurrentPlayer = "1" Then
            PawnCoefficent = Pawn1Coefficent
            RookCoefficent = Rook1Coefficent
            KnightCoefficent = Knight1Coefficent
            BishopCoefficent = Bishop1Coefficent
            QueenCoefficent = Queen1Coefficent
            MoveNumberCoefficent = Move1NumberCoefficent
        Else
            PawnCoefficent = Pawn2Coefficent
            RookCoefficent = Rook2Coefficent
            KnightCoefficent = Knight2Coefficent
            BishopCoefficent = Bishop2Coefficent
            QueenCoefficent = Queen2Coefficent
            MoveNumberCoefficent = Move2NumberCoefficent
        End If
        'get the correct variables
        Dim Start As Integer = 1
        If Player = "1" Then
            Start = 17
        End If
        Dim AP(0) As String
        For i = 1 To MoveTo.Length - 1
            For o = Start To Start + 15
                If MoveTo(i) = TempLocation(o) Then
                    Dim Found As Boolean = False
                    For p = 1 To AP.Length - 1
                        If TempLocation(o) = AP(p) Then
                            Found = True
                        End If
                    Next
                    If Found = False Then
                        Dim OPMoveFrom(0) As String
                        Dim OPMoveTo(0) As String
                        Dim OPS As Char = "2"
                        If Player = "2" Then
                            OPS = "1"
                        End If
                        Possible(OPS, TempBoard, TempLocation, TempNumMoves, OPMoveFrom, OPMoveTo, True)
                        Dim ProtectedPlaces(0) As String
                        For p = 1 To OPMoveTo.Length - 1
                            For a = 1 To AP.Length - 1
                                If OPMoveTo(p) = AP(a) Then
                                    Array.Resize(ProtectedPlaces, ProtectedPlaces.Length + 1)
                                    ProtectedPlaces(ProtectedPlaces.Length - 1) = OPMoveTo(p)
                                End If
                            Next
                        Next
                        If AP.Length > ProtectedPlaces.Length Then
                            For a = 1 To AP.Length - 1
                                Dim Founds As Boolean = False
                                For s = 1 To ProtectedPlaces.Length - 1
                                    If ProtectedPlaces(s) = AP(a) Then
                                        Founds = True
                                    End If
                                Next
                                If Founds = False Then
                                    'count up the values
                                    Select Case LCase(TempPieces(o)(3))
                                        Case "p"
                                            Value += PawnCoefficent
                                        Case "r"
                                            Value += RookCoefficent
                                        Case "n"
                                            Value += KnightCoefficent
                                        Case "b"
                                            Value += BishopCoefficent
                                        Case "q"
                                            Value += QueenCoefficent
                                    End Select
                                End If
                            Next
                        End If
                    End If
                End If
            Next
        Next
        Return Value
    End Function
    'calculate the interesting coefficients
    Function UpperPercentage()
        Dim Value As Integer
        'sort the interestings into order
        Quicksort(Interestings, 1, Interestings.Length - 1)
        'get the bottom number
        Dim BotInter As Integer = Interestings(1)
        If BotInter < 0 Then
            For i = 1 To Interestings.Length - 1
                Interestings(i) -= BotInter
            Next
        End If
        'gets 2 standard deviations away from the mean to get any anomolies and look deeper
        Value = Mean(Interestings) + 2 * (SD(Interestings))
        'reset the array
        Array.Resize(Interestings, 0)
        If BotInter < 0 Then
            Value += BotInter
        End If
        Return Value
    End Function
    'gets the mean
    Function Mean(ByVal Arr() As Integer) As Double
        Dim Value As Double = 0.0
        For i = 0 To Arr.Length - 1
            Value += Arr(i)
        Next
        Value /= Arr.Length
        Return Value
    End Function
    'gets the sum of the mean
    Function SumOf_XBar(ByVal Arr() As Integer)
        Dim Value As Double = 0
        For i = 1 To Arr.Length - 1
            Value += (Arr(i) - Mean(Arr)) ^ 2
        Next
        Return Value
    End Function
    'gets hte standar deviation
    Function SD(ByVal Arr() As Integer) As Double
        Dim Value As Double = 0.0
        Value = Math.Sqrt((SumOf_XBar(Arr) / Arr.Length - 2))
        Return Value
    End Function
    'sorst the array into order using quicksort algorithem
    Sub Quicksort(ByVal list() As Integer, ByVal min As Integer, ByVal max As Integer)
        Dim high As Integer
        Dim mid As Integer
        Dim low As Integer
        Dim i As Integer
        Dim random_number As New Random
        If min >= max Then Exit Sub
        i = random_number.Next(min, max + 1)
        mid = list(i)
        list(i) = list(min)
        low = min
        high = max
        Do
            Do While list(high) >= mid
                high = high - 1
                If high <= low Then Exit Do
            Loop
            If high <= low Then
                list(low) = mid
                Exit Do
            End If
            list(low) = list(high)
            low = low + 1
            Do While list(low) < mid
                low = low + 1
                If low >= high Then Exit Do
            Loop
            If low >= high Then
                low = high
                list(high) = mid
                Exit Do
            End If
            list(high) = list(low)
        Loop
        'recusion of the two smaller arrays
        Quicksort(list, min, low - 1)
        Quicksort(list, low + 1, max)
    End Sub
    'adding the winning or losing place
    Sub AddingToDataBase(ByVal AddedMoveFrom As String, ByVal AddedMoveTo As String, ByVal FileName As String, ByVal Add As Boolean)
        'see if the file exists
        If Not System.IO.File.Exists(MainFilePath + "Chess\Computer\" + FileName + ".txt") = True Then
            System.IO.File.Create(MainFilePath + "Chess\Computer\" + FileName + ".txt").Dispose()
        End If
        'get the boardstate
        Dim BoardState As String = BoardLine(Board)
        Dim BoardFound As Boolean = False
        'read all the lines in the file
        Dim Lines() As String = File.ReadAllLines(MainFilePath + "Chess\Computer\" + FileName + ".txt")
        For i = 0 To File.ReadAllLines(MainFilePath + "Chess\Computer\" + FileName + ".txt").Length
            Dim CurrentLine As String = ReadLineWithNumberFrom(MainFilePath + "Chess\Computer\" + FileName + ".txt", i)
            Dim SplitLine() As String = CurrentLine.Split(",")
            'compare the fileboards with the current one
            If BoardState = SplitLine(0) Then
                Dim Found = False
                For o = 1 To SplitLine.Length - 1
                    Dim CurrentMoves() As String = SplitLine(o).Split(":")
                    If AddedMoveFrom = CurrentMoves(1) And AddedMoveTo = CurrentMoves(2) Then
                        If Add = True Then
                            'added
                            Lines(i) = CurrentLine.Replace(CurrentMoves(0) + ":" + AddedMoveFrom + ":" + AddedMoveTo, Convert.ToString(1 + (Convert.ToInt16(CurrentMoves(0)))) + ":" + AddedMoveFrom + ":" + AddedMoveTo)
                        Else
                            If Convert.ToInt16(CurrentMoves(0)) - 1 > 0 Then
                                Lines(i) = CurrentLine.Replace(CurrentMoves(0) + ":" + AddedMoveFrom + ":" + AddedMoveTo, Convert.ToString((Convert.ToInt16(CurrentMoves(0)) - 1)) + ":" + AddedMoveFrom + ":" + AddedMoveTo)
                            Else
                                Lines(i) = CurrentLine.Replace("," + CurrentMoves(0) + ":" + AddedMoveFrom + ":" + AddedMoveTo, "")
                            End If
                        End If
                        Found = True
                    End If
                Next
                'add if the board is found
                If Found = False And Add = True Then
                    Lines(i) = CurrentLine + ",1:" + AddedMoveFrom + ":" + AddedMoveTo
                End If
                BoardFound = True
                Exit For
            End If
        Next
        'if it doesnt exist write a new line
        If BoardFound = False And Add = True Then
            Array.Resize(Lines, Lines.Length + 1)
            Lines(Lines.Length - 1) = BoardState + ",1:" + AddedMoveFrom + ":" + AddedMoveTo
        End If
        System.IO.File.WriteAllLines(MainFilePath + "Chess\Computer\" + FileName + ".txt", Lines)
    End Sub
    'the winning database
    Sub DataBaseResults(ByVal WinningPlayer As Char)
        If WritingDataBase = True Then
            'get the number of current number with the compressedmoves
            Dim Selected As Integer = File.ReadAllLines(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\CompressedMove.txt").Length - 1
            Dim ActualTotalMoves = Selected
            'get the correct playernumber
            Dim PlayerNumber As String = "1"
            If Selected Mod 2 = 0 Then
                PlayerNumber = "2"
            End If
            If PlayerNumber <> WinningPlayer Then
                Selected -= 1
            End If
            For i = Selected To 1 Step -1
                PlayerNumber = "1"
                If i Mod 2 = 0 Then
                    PlayerNumber = "2"
                End If
                Dim CorH As String = "Human"
                If GameMode = "CVC" Or PlayerNumber = "2" Then
                    CorH = "Computer"
                End If
                'load the games in review
                If GameMode = "CVH" Then
                    Load(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\Moves\" + Convert.ToString(i) + CorH + ".txt")
                Else
                    Load(MainFilePath + "Chess\" + GameMode + "\Games\" + CurrentGameFile + "\Moves\" + Convert.ToString(i) + CorH + PlayerNumber + ".txt")
                End If
                'display the review
                DisplayReview()
                Dim adding As Boolean = False
                If PlayerNumber = WinningPlayer Then
                    adding = True
                End If
                'gets the new database
                Dim Filename As String = "WhiteDataBase"
                If PlayerNumber = "2" Then
                    Filename = "BlackDataBase"
                End If
                'split them
                Dim ReasonSplit() As String = LoadingReason.Split(":")
                AddingToDataBase(ReasonSplit(0), ReasonSplit(1), Filename, adding)
            Next
        End If
    End Sub
    'change the colour
    Function ColourChange(ByVal Colour As String) As Integer
        'all the colours i want
        Dim Colours(15) As String
        Colours(1) = "DarkBlue"
        Colours(2) = "DarkGreen"
        Colours(3) = "DarkCyan"
        Colours(4) = "DarkRed"
        Colours(5) = "DarkMagenta"
        Colours(6) = "DarkYellow"
        Colours(7) = "Gray"
        Colours(8) = "DarkGray"
        Colours(9) = "Blue"
        Colours(10) = "Green"
        Colours(11) = "Cyan"
        Colours(12) = "Red"
        Colours(13) = "Magenta"
        Colours(14) = "Yellow"
        'menu
        Dim Entry As String = "UpArrow"
        Dim k As ConsoleKeyInfo
        Dim Selected As Integer = 2
        Do
            Select Case (Entry)
                Case "UpArrow"
                    If Not Selected = 1 Then
                        Selected -= 1
                    End If
                Case "DownArrow"
                    If Not Selected = 14 Then
                        Selected += 1
                    End If
            End Select
            Do
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("What colour do you want " + Colour + " to be? ")
                For i = 1 To 14
                    If Selected = i And i < 15 Then
                        Console.ForegroundColor = i
                        Console.WriteLine(Colours(i))
                    Else
                        Console.ForegroundColor = ConsoleColor.White
                        Console.WriteLine(Colours(i))
                    End If
                Next
                k = Console.ReadKey(True)
                Entry = k.Key.ToString
            Loop Until Entry = "UpArrow" Or Entry = "DownArrow" Or Entry = "Enter"
        Loop Until Entry = "Enter"
        Return Selected
    End Function
End Module






