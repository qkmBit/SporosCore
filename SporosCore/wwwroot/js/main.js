function changeCategory(event){
    var elem = document.getElementsByClassName('active')[0];
    //elem.classList.remove('Current');
    elem.className="flex-sm-fill text-sm-center nav-link";
    event.currentTarget.className ="flex-sm-fill text-sm-center nav-link active"; 
}
$(document).ready(function(){
    var elements = $('.svg-circle__dynamic-circle');
    var elemCategory = $('.elemCategory');
    var Advantages = $('.Advantages');
    $counter=0;
    $(window).scroll(function(){
        var scroll = $(window).scrollTop() + $(window).height();
        var scrollSec = $(window).scrollTop() + 65;
        var offsetCircle = elements.offset().top;
        var offsetCategory = elemCategory.offset().top;
        var offsetAdv = Advantages.offset().top;
        if(scrollSec>offsetCategory){
            elemCategory[0].style.position='fixed';
            elemCategory[0].style.top=0;
            elemCategory[0].classList.remove('elemCategory--new');
            $(".elemCategory").css('margin-top','0px');
            $(".Advantages")[0].classList.add('elemCategory--new');
            Advantages[0].style.paddingTop='65px';
            $(".elemCategory").css('box-shadow', '0 0 10px #4d4d4d');
        }
        if(scrollSec<offsetAdv){
            elemCategory[0].style.position='relative';
            elemCategory[0].style.top=0;
            elemCategory[0].classList.add('elemCategory--new');
            $(".Advantages")[0].classList.remove('elemCategory--new');
            Advantages[0].style.paddingTop='0px';
            $(".elemCategory").css('box-shadow', 'none');
        }
        if(scroll> offsetCircle && $counter==0)
        {
            for(var i=0;i<elements.length;i++){
                elements[i].style.strokeDasharray=elements[i].attributes[1].nodeValue+', '+ (100-elements[i].attributes[1].nodeValue);
            }
            $counter=1;
        }
    })
});
$(function(){
    $('.addrSel').change(function () {
        $('#CityDiv').css("display","flex");
        $('#AddressTxtDiv').css("display","flex");
        $("label[for='City'")[0].removeAttribute("hidden");
        $("label[for='AddressTxt'")[0].removeAttribute("hidden");
        curOpt = $('.addrSel :selected').val();
        if(curOpt=="Add"){
            $('#City')[0].removeAttribute("disabled");
            $('#AddressTxt')[0].removeAttribute("disabled");
            $('#City')[0].value="";
            $('#AddressTxt')[0].value="";
        }
        else{
            $('#City')[0].setAttribute("disabled", "");
            $('#AddressTxt')[0].setAttribute("disabled","");
        }
    })
});
function AuthClose(event){
    let parent = event.currentTarget.parentElement;
    while(parent.firstChild){
        parent.removeChild(parent.firstChild);
    }
    $(".modal").css({"visibility":"hidden"});
    $("*").css({"overflow":"visible"});
    $(".main-menu").css({"overflow":"hidden"});
}
function AuthOpen(event){
        $.get('/Home/OpenAuthWindow', function (data) {
            $('.AuthWindowFull').html(data);
        });
        $(".modal").css({"visibility":"visible"});
        $("*").css({"overflow":"hidden"})
}
function logInFailure(e) {
    alert("jxx");
    e.preventDefault();
}
function loginSubmit (event) {
        event.preventDefault();
        var email = $('#phone').val();
        var pass = $('#password').val();
        var check = $('#rememberMe').val();
        alert("jxx");
        while ($('.AuthWindowFull').firstChild) {
            $('.AuthWindowFull').removeChild(parent.firstChild);
        }
        $.ajax({
            url: '/Account/Login/',
            type: "POST",
            data: JSON.stringify({
                Email: email,
                Password: pass,
                RememberMe: check
            }),
            success: function (data) {
                $('.AuthWindowFull').html(data);
            }
        });
        $(".modal").css({ "visibility": "visible" });
        $("*").css({ "overflow": "hidden" });
}