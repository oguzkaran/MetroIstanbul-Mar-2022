CREATE TABLE TrackDatabase(

    No int Not Null,

    Station_Start_Position int,

    Station_End_Position int,

    Station_Name varchar(255),

    Track_ID int,

    Line_ID int,

    Track_Type int,

    Track_Start_Position int,

    Track_End_Position int,

    Track_Length int,

    Track_Speed_Limit int,

    Stopping_Point_Position_1 int,

    Stopping_Point_Type_1 int,

    Stopping_Point_Position_2 int,

    Stopping_Point_Type_2 int,

    Track_Connection_Entry_1 int,

    Track_Connection_Entry_2 int,

    Track_Connection_Exit_1 int,

    Track_Connection_Exit_2 int,

    X1_Point int,

    X2_Point int,

    Y1_Point int,

    Y2_Point int,

    Signal_ID_1 int,

    Signal_ID_2 int,

    PSD_ESB_ID_1 int,

    PSD_ESB_ID_2 int,

    Wayside_ID int,

    Primary Key(No)

);



CREATE TABLE SwitchDatabase(

    ID int primary key,

    Switch_ID int,

    Entry_Track int,

    Station_Name int,

    Left_Track int,

    Right_Track int,

    Normal_Position int

);