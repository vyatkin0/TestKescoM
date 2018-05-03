<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="TestWebApp._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Тест</title>
    <script type="text/javascript">

        function getsum() {

            //Получаем параметры запроса
            var request_params = "?i=0&j=0";
            var sURL = window.document.URL.toString();
            if (sURL.indexOf("?") > 0) {
                var arrParams = sURL.split("?");
                request_params="?"+arrParams[1];
            }
            else {
                alert("No Parameters Found");
                return;
            }

            var xmlHttp = new XMLHttpRequest();
            var url = "http://localhost:9879/Sum" + request_params;

            xmlHttp.open("GET", url, true);
            //xmlHttp.setRequestHeader("Content-Type", "application/json");
            xmlHttp.onreadystatechange = function () {
                if (xmlHttp.readyState == 4) {
                    document.getElementById("divValue").innerHTML = xmlHttp.responseText;
              }
            };

            var body = '';
            xmlHttp.send(body);
        }

      </script>
</head>
<body>
    <h1>Задание 1</h1>
    <a href="http://localhost:9879/RunNotepad">Запустить Notepad</a>

    <h1>Задание 2</h1>
    <a href="javascript:getsum();">Выбрать</a>
    <div id="divValue"></div>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
