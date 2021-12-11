

$(document).ready(function () {
    $("li").click(function () {
        var myClass = $(this).attr("class");

        if (myClass.toString() == "current dropdown") $(this).attr("class", "dropdown");
        if (myClass.toString() == "current") $(".current").attr("class", "");


        if (myClass.toString() == "") $(this).attr("class", "current");
        if (myClass.toString() == "dropdown") $(this).attr("class", "current dropdown");
        //alert(myClass);
    });
}


//$("li").click(function () {
//    var myClass = this.className;
//    alert(myClass);
//});

//$("li").click(function () {
//    var myClasses = this.classList;
//    alert(myClasses.length + " " + myClasses[0]);
//});