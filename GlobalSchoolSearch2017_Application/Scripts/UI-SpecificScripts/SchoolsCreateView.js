$(document).ready(function () {
    alert("Hi i am at 1");
    $("#CityName").prop("disabled", true);

    $("#CountryName").click(function() {
        if ($("#CountryName").val() != 0) {
            var cityOptions = {};
            cityOptions.url = "/Schools/getCities";
            cityOptions.type = "POST";
            cityOptions.data = JSON.stringify({ Country: $("#CountryName").val() });
            cityOptions.datatype = "json";
            cityOptions.contentType = "application/json";
            cityOptions.success = function(cityList) {
                $("#CityName").empty();
                for (var i = 0; i < cityList.length; i++) {
                    $("#CityName").append("<option>" + cityList[i] + "</option>");
                }
                $("#CityName").prop("disabled", false);

            };
            cityOptions.error = function() {
                alert(toString($("#CityName").val()));
                alert("Error in getting City List!!");
            };
            $.ajax(cityOptions);
        } else {
            $("#CityName").empty();
            $("#CityName").prop("disabled", true);
        }
    });
});