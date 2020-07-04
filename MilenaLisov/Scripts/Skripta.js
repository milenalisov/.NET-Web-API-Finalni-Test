$(document).ready(function () {

    var host = window.location.host;
    var token = null;
    var headers = {};
    var lanciEndPoint = "/api/lanci/";
    var hoteliEndPoint = "/api/hoteli/";

    loadingHoteli();

    function loadingHoteli() {
        $.getJSON("http://" + host + hoteliEndPoint, setHoteli);
    }

    $("body").on("click", "#btnDelete", deleteHotel);
    function setHoteli(data, status) {
        var container = $("#dataHotel");
        container.empty();
        if (status == "success") {
            var div = $("<div></div>");
            var table = $("<table class=table table-bordered></table>");
            var header;
            if (token) {
                header = $("<thead><tr><th>Naziv</th><th>Godina otvaranja</th><th>Broj soba</th><th>Broj zaposlenih</th><th>Lanac</th><th>Akcija</th></tr></thead>");
            } else {
                header = $("<thead><tr><th>Naziv</th><th>Godina otvaranja</th><th>Broj soba</th><th>Broj zaposlenih</th><th>Lanac</th></tr></thead>");
            }
            table.append(header);
            var tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                var row = "<tr>";
                var sadrzaj = "<td>" + data[i].Naziv + "</td><td>" + data[i].Godina + "</td><td>" + data[i].Sobe + "</td><td>" + data[i].Zaposleni + "</td><td>" + data[i].Lanac.Naziv + "</td>";
                var stringId = data[i].Id.toString();
                var content = "<td><button class='btn btn-link' id=btnDelete name=" + stringId + ">Brisanje</button></td>";


                if (token) {
                    row += sadrzaj + content + "</tr>";
                } else {
                    row += sadrzaj + "</tr>";
                }
                tbody.append(row);

            }
            table.append(tbody);
            div.append(table);
            container.append(div);

        } else {
            var divV = $("<div></div>");
            var hV = $("<h1>Greska prilikom ucitavanja hotela!</h1>");
            divV.append(hV);
            container.append(div);
        }

    }

    $("#reg").click(function () {
        $("#divRegIPri").css("display", "none");
        $("#registracijaDiv").css("display", "block");
    });

    $("#registracijaForm").submit(function (e) {
        e.preventDefault();
        var email = $("#userReg").val();
        var loz = $("#lozReg").val();
        var loz1 = $("#lezReg2").val();

        var dataSent = {
            "Email": email,
            "Password": loz,
            "ConfirmPassword": loz1
        };

        $.ajax({
            url: "http://" + host + "/api/Account/Register",
            type: "POST",
            data: dataSent
        }).done(function (data, status) {
            $("#uspesnaReg").css("display", "block");
            refreshForma();
        }).fail(function (data, status) {
            alert("Greska prilikom registracije!");
        });

    });

    function refreshForma() {
        $("#userReg").val("");
        $("#lozReg").val("");
        $("#lezReg2").val("");

        $("#userPri").val("");
        $("#lozPri").val("");

    }

    $("#odustajanje").click(function () {
        $("#uspesnaReg").css("display", "none");
        $("#divRegIPri").css("display", "block");
        $("#registracijaDiv").css("display", "none");
        refreshForma();
    });

    $("#pri").click(function () {
        $("#prijavaDiv").css("display", "block");
        $("#divRegIPri").css("display", "none");
    });

    $("#prijavaIzReg").click(function () {
        $("#prijavaDiv").css("display", "block");
        $("#uspesnaReg").css("display", "none");
        $("#registracijaDiv").css("display", "none");
    });

    $("#prijavaForm").submit(function (e) {
        e.preventDefault();
        var email = $("#userPri").val();
        var loz = $("#lozPri").val();

        var dataSent = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            url: "http://" + host + "/Token",
            type: "POST",
            data: dataSent
        }).done(function (data, status) {
            token = data.access_token;
            refreshForma();

            $("#divInfo").css("display", "block");
            $("#pretragaDiv").css("display", "block");
            $("#tradicija").css("display", "block");
            $("#divRegIPri").css("display", "none");
            $("#info").append("<b>Prijavljeni korisnik: </b>" + data.userName);
            $("#prijavaDiv").css("display", "none");
            $("#dodavanjeHotela").css("display", "block");
            refreshTable();

        }).fail(function (data, status) {
            alert("Greska prilikom prijave!");
        });

    });

    function refreshTable() {
        loadingHoteli();
        loadingLanci();
    }

    $("#odustajanje1").click(function () {
        $("#prijavaDiv").css("display", "none");
        refreshForma();
        $("#divRegIPri").css("display", "block");

    });

    $("#odjava").click(function () {
        token = null;
        headers = {};
        refreshTable();
        $("#divInfo").css("display", "none");
        $("#info").empty();
        $("#divRegIPri").css("display", "block");
        $("#pretragaDiv").css("display", "none");
        $("#tradicija").css("display", "none");
        $("#dodavanjeHotela").css("display", "none");

    });

    function deleteHotel() {
        var deleteId = this.name;
        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        $.ajax({
            url: "http://" + host + hoteliEndPoint + deleteId.toString(),
            type: "DELETE",
            headers: headers

        }).done(function (data, status) {
            refreshTable();

        }).fail(function (data, status) {
            alert("Desila se greska prilikom brisanja");
        });
    }

    $("#pretragaForm").submit(function (e) {
        e.preventDefault();
        var od = $("#najmanje").val();
        var dokle = $("#najvise").val();

        var validanOd = true;
        var validanDo = true;

        var validanOdnos = true;

        if (+od <= 9 || +od % 1 !== 0 || +od >= 1000 || isNaN(+od)) {
            validanOd = false;
        }

        if (+dokle <= 9 || +dokle % 1 !== 0 || +dokle >= 1000 || isNaN(+dokle)) {
            validanDo = false;
        }

        if (+od > +dokle) {
            validanOdnos = false;
        }
        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        if (validanOd && validanDo && validanOdnos) {
            var dataSent = {
                "najmanje": od,
                "najvise": dokle
            };
            $.ajax({
                url: "http://" + host + "/api/kapacitet",
                type: "POST",
                data: dataSent,
                headers: headers
            }).done(function (data, status) {

                $("#najmanje").val("");
                $("#najvise").val("");
                setHoteli(data, status);
            }).fail(function (data, status) {
                alert("Greska prilikom pretrage!");
                $("#najmanje").val("");
                $("#najvise").val("");
            });
        } else {
            alert("Greska prilikom pretrage!");
        }
    });

    $("#ucitavanjeTradicije").click(function () {

        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        $.ajax({
            url: "http://" + host + "/api/tradicija",
            type: "GET",
            headers: headers
        }).done(UcitavanjeTradicije)
            .fail(function (data, status) {
                alert("Doslo je do greske prilikom ucitavanja lanaca sa najduzom tradicijom!");
            });
    });

    function UcitavanjeTradicije(data, status) {
        $("#ucitavanjeTradicije").css("display", "none");
        if (status == "success") {
            var lista = $("#lanci");
            lista.empty();

            for (i = 0; i < data.length; i++) {

                var red = "<li><p><b>" + data[i].Naziv + "</b> (osnovan  <b>" + data[i].Godina + "</b>.godine)</p></li>";
                lista.append(red);

            }


        } else {
            alert("Doslo je do greske prilikom ucitavanja lanaca sa najduzom tradicijom!");
        }
    }

    function loadingLanci() {
        $.getJSON("http://" + host + lanciEndPoint, setLanci);
    }

    function setLanci(data, status) {
        var input = $("#lanacHotel");
        input.empty();
        if (status == "success") {

            for (i = 0; i < data.length; i++) {
                var opcija = "<option value=" + data[i].Id + ">" + data[i].Naziv + "</option>";
                input.append(opcija);
            }

        } else {
            alert("Neuspedno ucitavanje lanaca!");
        }

    }

    $("#dodavanjeForm").submit(function (e) {
        e.preventDefault();

        var naziv = $("#naziv").val();
        var godina = $("#otvaranje").val();
        var lanac = $("#lanacHotel").val();
        var sobe = $("#sobe").val();
        var sobePolje = $("#sobe");
        var zaposleni = $("#zaposleni").val();

        var validanNaziv = true;
        var validnaGodina = true;
        var validneSobe = true;
        var validnoZaposleni = true;

        if (+naziv > 80) {
            validanNaziv = false;
        }

        if (isNaN(+godina) || +godina % 1 !== 0 || +godina < 1950 || +godina > 2020) {
            validnaGodina = false;
        }

        if (sobe === null || sobe === undefined || sobe === "") {
            validneSobe = true;

        } else if (+sobe <= 9 || +sobe >= 1000 || isNaN(+sobe) || +sobe % 1 !== 0) {
            validneSobe = false;
        }

        if (+zaposleni % 1 !== 0 || isNaN(+zaposleni) || +zaposleni <= 1) {
            validnoZaposleni = false;
        }

        if (validanNaziv && validnaGodina && validneSobe && validnoZaposleni) {
            var DataSent = {
                "Naziv": naziv,
                "Godina": godina,
                "Zaposleni": zaposleni,
                "Sobe": sobe,
                "LanacId": lanac
            };

            if (token) {
                headers.Authorization = "Bearer " + token;
            }

            $.ajax({
                url: "http://" + host + hoteliEndPoint,
                type: "POST",
                data: DataSent,
                headers: headers
            }).done(function (data, status) {
                refreshTable();
                $("#naziv").val("");
                $("#otvaranje").val("");
                $("#lanacHotel").val("");
                $("#sobe").val("");
                $("#zaposleni").val("");
            }).fail(function (data, status) {
                alert("Greska prilikom dodavanja!");
            });
        } else {
            alert("Greska prilikom dodavanja!");
        }

    });

    $("#odustajanje3").click(function () {
        $("#naziv").val("");
        $("#otvaranje").val("");
        $("#lanacHotel").val("");
        $("#sobe").val("");
        $("#zaposleni").val("");
    });
});