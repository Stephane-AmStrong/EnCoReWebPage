function ProfilRefrech() {
    $("#Tout").parent("li").removeClass();
    $("#Tout").removeClass();
    $("#Direction").parent("li").removeClass();
    $("#Direction").removeClass();
    $("#Superviseur").parent("li").removeClass();
    $("#Superviseur").removeClass();
    $("#Visiteur").parent("li").removeClass();
    $("#Visiteur").removeClass();
};

function Profil(id) {
    $('.i-poPup').removeClass('open');
    $('#poPup-bg').remove();

    ProfilRefrech();
    $("#" + id).addClass("active");
    var text = $("#" + id).text();
    document.getElementById("choix_profil").innerHTML = text;
    profil = id;

    ListeRefrech();
};

function StatutRefrech() {
    $("#Tout").parent("li").removeClass();
    $("#Tout").removeClass();
    $("#Actif").parent("li").removeClass();
    $("#Actif").removeClass();
    $("#Bloque").parent("li").removeClass();
    $("#Bloque").removeClass();
};


function Statut(id) {
    $('.i-poPup').removeClass('open');
    $('#poPup-bg').remove();

    StatutRefrech();
    $("#" + id).addClass("active");
    var text = $("#" + id).text();
    document.getElementById("choix_statut").innerHTML = text;
    statut = id;
    ListeRefrech();
};

function TypeClientRefrech() {
    $("#Tout_Type").parent("li").removeClass();
    $("#Tout_Type").removeClass();
    $("#Corps_medicaux").parent("li").removeClass();
    $("#Corps_medicaux").removeClass();
    $("#Pharmacies").parent("li").removeClass();
    $("#Pharmacies").removeClass();
};

function TypeClient(id) {
    $('.i-poPup').removeClass('open');
    $('#poPup-bg').remove();

    TypeClientRefrech();
    $("#" + id).addClass("active");
    var text = $("#" + id).text();
    document.getElementById("choix_type").innerHTML = text;
    type = id;
    ListeRefrech();
};

function SpecialiteClient(id) {
    $('.i-poPup').removeClass('open');
    $('#poPup-bg').remove();

    var text = $("#" + id).text();
    document.getElementById("choix_specialite").innerHTML = text;
    document.getElementById("specialite_choisie").innerHTML = text;
    specialite = id;
    ListeRefrech();
};

function AskDeleteConfirmation(actionName, id, tagId) {
    alert('passed');
    $.ajax({
        url: actionName,
        data: {
            id: id
        },
        cache: false,
        success: function (data) {
            $(tagId).html(data);

            $('.i-poPup').removeClass('open');
            $('#poPup-bg').remove();
            $('body').append('<div id="poPup-bg"> </div>');
            $(tagId).addClass('open');
            $(tagId).prepend('<a href="#" class="i-poPup-close"></a>');
        }
    })
};

function Delete(actionName, id) {
    $.ajax({
        type: "POST",
        url: actionName,
        data: {
            id: id
        },
        cache: false,
        success: function (data) {
            if (data == "OK") {
                $('.i-poPup').removeClass('open');
                $('#poPup-bg').remove();
                ListeRefrech();
            }
        }
    })
};

function ShowGestion(actionName, id, tagId) {
    alert('passed');
    $.ajax({
        url: actionName,
        data: {
            id: id
        },
        cache: false,
        success: function (data) {
            $(tagId).html(data);

            $('.i-poPup').removeClass('open');
            $('#poPup-bg').remove();
            $('body').append('<div id="poPup-bg"> </div>');
            $(tagId).addClass('open');
            $(tagId).prepend('<a href="#" class="i-poPup-close"></a>');
        }
    })
};

function ShowPopup(actionName, id, tagId) {
    alert('passed');
    $.ajax({
        url: actionName,
        data: {
            id: id
        },
        cache: false,
        success: function (data) {
            $(tagId).html(data);

            $('.i-poPup').removeClass('open');
            $('#poPup-bg').remove();
            $('body').append('<div id="poPup-bg"> </div>');
            $(tagId).addClass('open');
            $(tagId).prepend('<a href="#" class="i-poPup-close"></a>');
        }
    })
};

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 111 && charCode != 58 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}