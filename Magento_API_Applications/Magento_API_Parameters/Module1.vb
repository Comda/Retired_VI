
Imports System.Threading
    Module Module1
        Sub Main()
            Dim theThread _
            As New Threading.Thread(
            AddressOf TestMultiThreading)
            theThread.Start(5)
        End Sub
        Public Sub TestMultiThreading(ByVal X As Long)
            For loopCounter As Integer = 1 To 10
                X = X * 5 + 2
                Console.WriteLine(X)
            Next
            Console.ReadLine()
        End Sub
    End Module

