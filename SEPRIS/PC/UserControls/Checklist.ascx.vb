Public Class Checklist
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property Area
        Get
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID)
            Return usuario.IdArea
        End Get
    End Property

    Public ReadOnly Property EstatusPC
        Get
            Dim PC As New Entities.PC(Folio)
            PC = Session("PC")
            Return PC.IdEstatus
        End Get
    End Property

    <System.ComponentModel.Browsable(False)>
    Public Property ControlesResolucion As Boolean
        Get
            Return pnlResolucion.Visible
            Return RequiredFieldValidator6.Enabled
        End Get
        Set(value As Boolean)
            pnlResolucion.Visible = value
            RequiredFieldValidator6.Enabled = value
        End Set
    End Property

    <System.ComponentModel.ReadOnly(False)>
    Public Property ControlesResolucionHabiliado As Boolean
        Get
            Return pnlResolucion.Enabled
        End Get
        Set(value As Boolean)
            pnlResolucion.Enabled = value
        End Set
    End Property


    <System.ComponentModel.Browsable(False)>
    Public Property ControlesComentariosVisible As Boolean
        Get
            Return pnl_Comentarios.Visible
        End Get
        Set(value As Boolean)
            pnl_Comentarios.Visible = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False)>
    Public Property ControlesComentariosHabilitados As Boolean
        Get
            Return pnl_Comentarios.Enabled
        End Get
        Set(value As Boolean)
            pnl_Comentarios.Enabled = value
        End Set
    End Property


    Public ReadOnly Property PC As Entities.PC
        Get
            Return DirectCast(Session("PC"), Entities.PC)
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return Session("ID_USR")
        End Get
    End Property



    Public Sub Inicializar()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklist();", True)
    End Sub

    Public Sub InicializarFolio()
        If PC.IdEstatus = 8 Then
            RequerimientoInformacion.Visible = True
            RequerimientoInformacion.Inicializar(Folio)
            pnlResolucion.Visible = False
            pnl_Comentarios.Visible = False
        End If
        If PC.IdEstatus = 9 Then
            RequerimientoInformacion.Visible = False
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklistFolio();", True)
    End Sub

    Public Sub InicializarFolioRes(Check As String, pnl As Integer)
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID)

        If usuario.IdArea = 36 Then
            trSubFolios.Visible = True
        End If

        If pnl = 1 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklistFolioRes('" + Check + "','1');", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklistFolioRes('" + Check + "','0');", True)
        End If

        Select Case PC.IdEstatus
            Case 7
                If Not PC.Cumple Then

                    If Not (ddl_Resolucion.Items.Count = 3) Then
                        ddl_Resolucion.Items.RemoveAt(2)
                        ddl_Resolucion.Items.RemoveAt(2)
                    End If
                End If
            Case 9
                Dim dtRequerimientos As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
                If dtRequerimientos.Rows.Count > 0 Then
                    RequerimientoInformacion.Visible = True
                End If

                If (PC.IdResolucion = 3) Then
                    ddl_Resolucion.Items.RemoveAt(3)
                Else
                    Dim dt_Actividad As DataTable = Actividad.ObtenerTodas(Folio)
                    If dt_Actividad.Select("I_ID_ESTATUS = 'Completa'").Count > 0 Then
                        Dim row As DataTable = dt_Actividad.Select("I_ID_ESTATUS = 'Completa'").CopyToDataTable
                        If dt_Actividad.Rows.Count = row.Rows.Count Then
                            ddl_Resolucion.Items.RemoveAt(3)
                        End If
                    End If
                End If
                If Not PC.Cumple Then
                    ddl_Resolucion.Items.RemoveAt(2)
                    ddl_Resolucion.Items.RemoveAt(2)
                End If
            Case 108
                If Not ddl_Resolucion.Items.FindByValue("3") Is Nothing Then
                    ddl_Resolucion.Items.RemoveAt(3)
                End If
        End Select

    End Sub

    Public Sub InicializarFolioResOk()
        Dim usuario As New Entities.Usuario()
        Dim evento As String = ""
        usuario = Session(Entities.Usuario.SessionID)
        Dim ComentariosAdicionales = ""

        If usuario.IdArea = 36 Then
            trSubFolios.Visible = True
        End If

        If EstatusPC >= 9 Then
            Dim dtRequerimientos As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
            If dtRequerimientos.Rows.Count > 0 Then
                RequerimientoInformacion.Visible = True
            End If
        End If

        Select Case PC.IdEstatus
            '    Case 101
            '        txtMotivoNo.Text = PC.DescripcionNoCuemple
            '        txtMotivoNo.Enabled = False

            Case 102
                Session("ddl_Resolucion") = String.Empty
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                ddl_Resolucion.SelectedIndex = PC.IdResolucion
                ddl_Resolucion.Enabled = False
                pnlResolucion.Enabled = False
                txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                txtMotivoNo.Enabled = False
                'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"

                If (Session("ddl_Resolucion") = String.Empty) Then
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                End If
            Case 103
                Session("ddl_Resolucion") = String.Empty
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                ddl_Resolucion.SelectedIndex = PC.IdResolucion
                ddl_Resolucion.Enabled = False
                pnlResolucion.Enabled = False
                txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                txtMotivoNo.Enabled = False
                evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"

                If (Session("ddl_Resolucion") = String.Empty) Then
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                End If

            Case 104
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                txtMotivoNo.Enabled = False
            'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"

            Case 105 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 4
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        txtMotivoNo.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                End Select

            Case 106 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 4
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        txtMotivoNo.Enabled = False
                        ddl_Resolucion.Enabled = False
                        txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                End Select
            Case 15 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 5
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        txtMotivoNo.Enabled = False
                        ddl_Resolucion.Visible = True
                        ddl_Resolucion.Enabled = False
                        txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                End Select
            Case 10
                Select Case PC.IdPaso
                    Case 2
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'txtMotivoNo.Enabled = False
                        Dim thot As String = txtMotivoNo.Text
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = True
                        pnlResolucion.Enabled = True
                        txt_ComentariosResolucion.Enabled = True

                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                        'txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"


                End Select


            Case 21
                Select Case PC.IdPaso
                    Case 2
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'txtMotivoNo.Enabled = False
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = True
                        pnlResolucion.Visible = False
                        pnl_Comentarios.Visible = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"


                End Select

        End Select

        If PC.ComentariosAdicionales = String.Empty Then
            ComentariosAdicionales = ""
        Else
            ComentariosAdicionales = PC.ComentariosAdicionales.ToString()
        End If

        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklistFolioResOk('" + PC.IdResolucion.ToString() + "','" + PC.DescripcionResolucion.ToString() + "','" + ComentariosAdicionales + "','" + evento + "');", True)
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar checklist", "CargarChecklist()", True)

        'Page.ClientScript.RegisterStartupScript(Page.GetType(), "UniqueKeyForThisScript", "alert('HELLO');", True)

    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'InicializarFolioResOk()

            'Dim ComentariosAdicionales = String.Empty
            'Dim evento As String = String.Empty

            'If PC.ComentariosAdicionales = String.Empty Then
            '    ComentariosAdicionales = ""
            'Else
            '    ComentariosAdicionales = PC.ComentariosAdicionales.ToString()
            'End If

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Cargar checklist", "CargarChecklist()", True)


            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistFolioResOk10()", True)


        End If




        Dim usuario As New Entities.Usuario()
        Dim evento As String = ""
        usuario = Session(Entities.Usuario.SessionID)
        Dim ComentariosAdicionales = ""

        If usuario.IdArea = 36 Then
            trSubFolios.Visible = True
        End If

        If EstatusPC >= 9 Then
            Dim dtRequerimientos As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
            If dtRequerimientos.Rows.Count > 0 Then
                RequerimientoInformacion.Visible = True
            End If
        End If

        Select Case PC.IdEstatus
            '    Case 101
            '        txtMotivoNo.Text = PC.DescripcionNoCuemple
            '        txtMotivoNo.Enabled = False

            Case 102
                Session("ddl_Resolucion") = String.Empty
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                ddl_Resolucion.SelectedIndex = PC.IdResolucion
                ddl_Resolucion.Enabled = False
                pnlResolucion.Enabled = False
                txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                txtMotivoNo.Enabled = False
                evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                If (Session("ddl_Resolucion") = String.Empty) Then
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                Else
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                End If
            Case 103
                Session("ddl_Resolucion") = String.Empty
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                ddl_Resolucion.SelectedIndex = PC.IdResolucion
                ddl_Resolucion.Enabled = False
                pnlResolucion.Enabled = False
                txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                txtMotivoNo.Enabled = False
                txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                If (Session("ddl_Resolucion") = String.Empty) Then
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                Else
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                End If

            Case 104
                Session("ddl_Resolucion") = String.Empty
                txtMotivoNo.Text = PC.DescripcionNoCuemple
                ddl_Resolucion.SelectedIndex = PC.IdResolucion
                ddl_Resolucion.Enabled = False
                pnlResolucion.Enabled = False
                txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                txtMotivoNo.Enabled = False
                txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                If (Session("ddl_Resolucion") = String.Empty) Then
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                Else
                    Session("ddl_Resolucion") = ddl_Resolucion.Text
                End If

            Case 105 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 4
                        Session("ddl_Resolucion") = String.Empty
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = False
                        pnlResolucion.Enabled = False
                        txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                        txtMotivoNo.Enabled = False
                        txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                        txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If
                End Select

            Case 106 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 4
                        Session("ddl_Resolucion") = String.Empty
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = False
                        pnlResolucion.Enabled = False
                        txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                        txtMotivoNo.Enabled = False
                        txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                        txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If


                        If (ddl_Resolucion.SelectedIndex = 4) Then
                            RequerimientoInformacion.Visible = False
                        End If
                End Select
            Case 108 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 3
                        Session("ddl_Resolucion") = String.Empty

                        If (ddl_Resolucion.Items.Count <= 4) Then
                            txtMotivoNo.Text = PC.DescripcionNoCuemple

                            ddl_Resolucion.Enabled = True
                            pnlResolucion.Enabled = True
                            txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                            txtMotivoNo.Enabled = False
                            txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                            txt_ComentariosAdicionales.Enabled = False
                            'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                            If (Session("ddl_Resolucion") = String.Empty) Then
                                Session("ddl_Resolucion") = ddl_Resolucion.Text
                            Else
                                Session("ddl_Resolucion") = ddl_Resolucion.Text
                            End If

                        Else
                            txtMotivoNo.Text = PC.DescripcionNoCuemple
                            ddl_Resolucion.SelectedIndex = PC.IdResolucion
                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Enabled = False
                            txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                            txtMotivoNo.Enabled = False
                            txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                            txt_ComentariosAdicionales.Enabled = False
                            'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                            If (Session("ddl_Resolucion") = String.Empty) Then
                                Session("ddl_Resolucion") = ddl_Resolucion.SelectedIndex
                            Else
                                Session("ddl_Resolucion") = ddl_Resolucion.SelectedIndex
                            End If


                            If (ddl_Resolucion.SelectedIndex = 4) Then
                                RequerimientoInformacion.Visible = False
                            End If
                        End If



                End Select
            Case 15 'MMOB - NUEVO CASO
                Select Case PC.IdPaso

                    Case 5
                        Session("ddl_Resolucion") = String.Empty
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = False
                        pnlResolucion.Enabled = False
                        txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                        txtMotivoNo.Enabled = False
                        txt_ComentariosAdicionales.Text = PC.ComentariosAdicionales
                        txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If
                End Select
            Case 10
                Select Case PC.IdPaso
                    Case 2
                        Session("ddl_Resolucion") = String.Empty
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'txtMotivoNo.Enabled = False
                        Dim thot As String = txtMotivoNo.Text
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = True
                        pnlResolucion.Enabled = True
                        txt_ComentariosResolucion.Enabled = True
                        txt_ComentariosResolucion.Text = PC.DescripcionResolucion
                        If Not PC.Cumple Then

                            If Not (ddl_Resolucion.Items.Count = 3) Then
                                ddl_Resolucion.Items.RemoveAt(2)
                                ddl_Resolucion.Items.RemoveAt(2)
                            End If
                        End If

                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = PC.IdResolucion
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)


                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)
                        'txt_ComentariosAdicionales.Enabled = False
                        'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"


                End Select


            Case 21
                Select Case PC.IdPaso
                    Case 2
                        'Session("ddl_Resolucion") = String.Empty
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        'ddl_Resolucion.Enabled = True
                        'pnlResolucion.Visible = False
                        'pnl_Comentarios.Visible = False
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)

                        If (PC.IdEstatusAnt = 4) Then

                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                        ElseIf (PC.IdEstatusAnt = 5)
                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                            txtMotivoNo.Enabled = False
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistEstatus21()", True)
                        ElseIf (PC.IdEstatusAnt = 6)
                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                            txtMotivoNo.Enabled = False
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistEstatus21()", True)
                        Else
                            txtMotivoNo.Text = PC.DescripcionNoCuemple
                            ddl_Resolucion.SelectedIndex = PC.IdResolucion
                            ddl_Resolucion.Enabled = True
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                        End If
                End Select

            Case 22
                Select Case PC.IdPaso
                    Case 2
                        'Session("ddl_Resolucion") = String.Empty
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        'ddl_Resolucion.Enabled = True
                        'pnlResolucion.Visible = False
                        'pnl_Comentarios.Visible = False
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistBloquear()", True)

                        If (PC.IdEstatusAnt = 4) Then

                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                        ElseIf (PC.IdEstatusAnt = 5)
                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistEstatus21()", True)
                        ElseIf (PC.IdEstatusAnt = 6)
                            ddl_Resolucion.Enabled = False
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklistEstatus21()", True)
                        Else
                            txtMotivoNo.Text = PC.DescripcionNoCuemple
                            ddl_Resolucion.SelectedIndex = PC.IdResolucion
                            ddl_Resolucion.Enabled = True
                            pnlResolucion.Visible = False
                            pnl_Comentarios.Visible = False
                        End If
                End Select

            Case 8
                Select Case PC.IdPaso
                    Case 2
                        'txtMotivoNo.Text = PC.DescripcionNoCuemple
                        'txtMotivoNo.Enabled = False
                        txtMotivoNo.Visible = False
                        lblMotivoNoCimplimiento.Visible = False
                        ddl_Resolucion.SelectedIndex = PC.IdResolucion
                        ddl_Resolucion.Enabled = True
                        Dim parentPanel As GridView = Me.RequerimientoInformacion.FindControl("gvReqInformac")
                        'Dim lst As List(Of GridViewRow) = parentPanel.Rows.Cast(Of GridViewRow).AsEnumerable.ToList
                        'Dim valor As String = parentPanel.SelectedRow.Cells("DESC_ESTATUS").Text

                        'pnlResolucion.Visible = True
                        'pnl_Comentarios.Visible = True
                        If Not PC.Cumple Then
                            ddl_Resolucion.Items.RemoveAt(2)
                        End If

                    'evento = "Mensajes(" + Chr(34) + "Se guardara la información capturada. ¿Deseas continuar?" + Chr(34) + ");"
                    Case 4
                        Dim parentPanel As GridView = Me.RequerimientoInformacion.FindControl("gvReqInformac")

                End Select

            Case 4
                Select Case PC.IdPaso
                    Case 2
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklist()", True)


                End Select


            Case 9
                Select Case PC.IdPaso
                    Case 2
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklist()", True)
                        txtMotivoNo.Text = PC.DescripcionNoCuemple
                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If
                End Select
            Case 7
                Select Case PC.IdPaso
                    Case 2
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cargar", "CargarChecklist()", True)

                        If (Session("ddl_Resolucion") = String.Empty) Then
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        Else
                            Session("ddl_Resolucion") = ddl_Resolucion.Text
                        End If
                End Select
        End Select

        If PC.ComentariosAdicionales = String.Empty Then
            ComentariosAdicionales = ""
        Else
            ComentariosAdicionales = PC.ComentariosAdicionales.ToString()
        End If



    End Sub
    Public Sub VisibleRequerimientos()
        RequerimientoInformacion.Visible = False
    End Sub
    Protected Sub ddl_Resolucion_SelectedIndexChanged(sender As Object, e As EventArgs)
        Session("ddl_Resolucion") = ddl_Resolucion.SelectedItem.Text
        'Response.Redirect("~/PC/DetallePC.aspx", False)
    End Sub

End Class