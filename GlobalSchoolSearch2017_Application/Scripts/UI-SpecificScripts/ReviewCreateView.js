$(document).ready(function () {
    alert("Hi I am at 2");
    $("#SchoolName1").prop("disabled", true);
    $("#CityName").prop("disabled", true);
   

    $("#CountryName").click(function () {

        if ($("#CountryName").val() != 0) {
            var CityOptions = {};
            CityOptions.url = "/Reviews/GetCities";
            CityOptions.type = "POST";
            CityOptions.data = JSON.stringify({ Country: $("#CountryName").val() });
            CityOptions.datatype = "json";
            CityOptions.contentType = "application/json";
            CityOptions.success = function (CityList) {
                $("#CityName").empty();
                for (var i = 0; i < CityList.length; i++) {
                    $("#CityName").append("<option>" + CityList[i] + "</option>");
                }
                $("#CityName").prop("disabled", false);
            };
            CityOptions.error = function () {
                alert(toString($("#CityName").val()));
                alert("Error in getting City List!!");
            };
            $.ajax(CityOptions);
            $("#CityName").change(function () {
                alert("I am at 3");
                if ($("#CityName").val() != 0) {
                    var schoolOptions = {};
                    schoolOptions.url = "/Reviews/GetSchools";
                    schoolOptions.type = "POST";
                    schoolOptions.data = JSON.stringify({ City: $("#CityName").val() });
                    schoolOptions.datatype = "json";
                    schoolOptions.contentType = "application/json";
                    schoolOptions.success = function (schoolList) {
                        $("#SchoolName").empty();
                        for (var i = 0; i < schoolList.length; i++) {
                            $("#SchoolName").append("<option>" + schoolList[i] + "</option>");
                        }
                        $("#SchoolName").prop("disabled", false);
                        alert("I am at 4");

                    };
                    schoolOptions.error = function () {
                        alert(toString($("#SchoolName").val()));
                        alert("Error in getting School List!!");
                        alert("I am at 5");
                    };
                    $.ajax(schoolOptions);
                }
            });



        } else {
            $("#CityName").empty();
            $("#CityName").prop("disabled", true);
        }
    });


});