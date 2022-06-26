function changeCategory(event) {
    var id = event.currentTarget.id;
    var elem = document.getElementsByClassName('active')[0];
    //elem.classList.remove('Current');
    elem.className="flex-sm-fill text-sm-center nav-link";
    event.currentTarget.className = "flex-sm-fill text-sm-center nav-link active";
    $.ajax({
        url: 'ChangeStore',
        type: "Post",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            id: id
        },
        success: function (data) {
            $(".swap").html(data);
        }
    });
}
$(document).ready(function(){
    var elements = $('.svg-circle__dynamic-circle');
    var elemCategory = $('.elemCategory');
    var Advantages = $('.Advantages');
    $counter=0;
    $(window).scroll(function (e) {
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
            $('#AddressTxt')[0].value = "";
            $('#delBtnDiv').css("display", "none");
        }
        else {
            $.ajax({
                url: 'GetAddress',
                type: "Post",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                dataType:"json",
                data: {
                    id: curOpt
                },
                success: function (data) {
                    $('#City')[0].value = data.City;
                    $('#AddressTxt')[0].value = data.Address1
                }
            });
            $('#City')[0].setAttribute("disabled", "");
            $('#AddressTxt')[0].setAttribute("disabled", "");
            $('#delBtnDiv').css("display", "flex");
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
function loginSubmit (event) {
        event.preventDefault();
        var email = $('#phone').val();
        var pass = $('#password').val();
        var check = $('#rememberMe')[0].checked;
        while ($('.AuthWindowFull').firstChild) {
            $('.AuthWindowFull').removeChild(parent.firstChild);
        }
        $.ajax({
            url: "../Account/Login",
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                Email: email,
                Password: pass,
                RememberMe: check
            },
            success: function (data) {
                $('.AuthWindowFull').html(data);
            }
        });
        $(".modal").css({ "visibility": "visible" });
        $("*").css({ "overflow": "hidden" });
}
$(function () {
    $('.submitChanges').click(function () {
        let needAddAddress = false;
        let picked = false;
        curOpt = $('.addrSel :selected').val();
        if (curOpt == "Add") {
            needAddAddress = true;
        }
        else if (curOpt == "NotPicked") {
            picked = true;
        }
            $.ajax({
                url: 'SaveProfile',
                type: "Post",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: {
                    FirstName: $('#Name').val(),
                    SecondName: $('#Surname').val(),
                    Patronymic: $('#Midname').val(),
                    CompanyName: $('#Organization').val(),
                    City: $('#City').val(),
                    Address: $('#AddressTxt').val(),
                    Email: $('#Email').val(),
                    PhoneNumber: $('#Phone').val(),
                    AddressAdd: needAddAddress,
                    Picked: picked,
                    AddressId: curOpt
                },
                success: function (data) {
                    var newDoc = document.open("text/html", "replace");
                    newDoc.write(data);
                    newDoc.close();
                }
            });
    })
});
$(function () {
    $('.del').click(function () {
        curOpt = $('.addrSel :selected').val();
        $.ajax({
            url: 'DeleteAddress',
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                AddressId: curOpt
            },
            success: function (data) {
                var newDoc = document.open("text/html", "replace");
                newDoc.write(data);
                newDoc.close();
            }
        });
    })
});
$(function () {
    $('.change').click(function () {

        $('#Name')[0].removeAttribute("disabled");
        $('#Surname')[0].removeAttribute("disabled");
        $('#Midname')[0].removeAttribute("disabled");
        $('#Organization')[0].removeAttribute("disabled");
        $('#City')[0].removeAttribute("disabled");
        $('#AddressTxt')[0].removeAttribute("disabled");
        $('#Phone')[0].removeAttribute("disabled");
        $('#Email')[0].removeAttribute("disabled");
    })
});
$(function () {
    $('.send').click(function () {
        $.ajax({
            url: 'Email',
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {},
            success: function (data) {
                $('.reqEmailConfirm').text(data)
            }
        });
    })
});
$(function () {
    $(document).on("click", '.cartIcon',function () {
        $('#cartCheck')[0].checked = !$('#cartCheck')[0].checked;
        if ($('#cartCheck')[0].checked) {
            $.ajax({
                url: '../Home/Cart',
                type: "Post",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: {

                },
                success: function (data) {
                    $('.cart').html(data);
                    $('.cart').css({ "display": "flex" });
                    $('.cart').css({ "height": "50%" });
                    $('.cart').css({ "padding": "5px" });
                    $('.cart').css({ "border": "black solid 1px" });
                    $('.cart').css({ "width": "25%" });
                }
            });
        }
        else {
            var cartItems = document.getElementsByClassName('cartItems');
            var CartItems = [];
            var cartList = [];
            for (let i = 0; i < cartItems.length; i++) {
                CartItems = {
                    CartId: cartItems[i].querySelector(".hiddenItemId").attributes.cartId.nodeValue,
                    ItemId: cartItems[i].querySelector(".hiddenItemId").attributes.id.nodeValue,
                    Count: cartItems[i].querySelector("#count").value
                }
                cartList.push(CartItems)
            }
            $.ajax({
                url: '../Store/CartChangeCount',
                type: "Post",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: {
                    cartItems: cartList
                },
                success: function (data) {
                }
            });
            $('.cart').html("");
            $('.cart').css({ "padding": "0px" });
            $('.cart').css({ "height": "0px" });
            $('.cart').css({ "width": "0px" });
            $('.cart').delay(5000).css({ "border": "none" });
        }
    })
});
$(document).on('wheel', function (e) {
    if (e.originalEvent.wheelDelta <= 0 && !$('#cartCheck')[0].checked) {
        $('.cartIcon').css({ 'margin-bottom': '-90px' });
        $('.cartCountIcon').css({ 'margin-bottom': '-90px' });
    }
    else {
        $('.cartIcon').css({ 'margin-bottom': '5px' });
        $('.cartCountIcon').css({ 'margin-bottom': '5px' });
    }
});
$(document).ready(function () {
    $(window).scroll(function (e) {
    })
});
function goToBlock(DivName) {
    var y = $('.' + DivName).offset().top - 65;
    window.scrollTo(0,y);
}
function changeCheck() {
    $('#checkmenu')[0].checked = !$('#checkmenu')[0].checked
};
$(function () {
    $(document).on("click", '.cartDelBtn' , function () {
        var count = $('.cartCountIcon').html();
        $.ajax({
            url: '../Store/DeleteFromCart',
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                itemId: (this).attributes.itemId.nodeValue,
                cartId: (this).attributes.cartId.nodeValue
            },
            success: function (data) {
                $('.cart').html(data);
                $('.cartCountIcon').html(count - 1);
            }
        });
    })
});
$(function () {
    $(document).on("change", '#count', function (e) {
        
        var count = e.currentTarget.parentElement.parentElement.attributes.id.nodeValue;
        var price = e.currentTarget.parentElement.parentElement.querySelector("#count").value;
        var oldPrice = e.currentTarget.parentElement.parentElement.querySelector(".price").value;
        var newFullPrice = 0;
        var prices = document.getElementsByClassName("price");
        e.currentTarget.parentElement.parentElement.querySelector(".price").value = count * price;
        for (var i = 0; i < prices.length; i++){
        newFullPrice = newFullPrice + Number(prices[i].value);
        }
        $("#fullPrice").html("Итого: " + newFullPrice);
    })
});
$(function () {
    $(document).on("click", '#clear', function (e) {
        $.ajax({
            url: '../Store/DeleteAllFromCart',
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                cartId: (this).attributes.cartId.nodeValue
            },
            success: function (data) {
                $('.cart').remove();
                $('.cartCountIcon').remove();
                $('.cartIcon').remove();
            }
        });
    })
});
$(function () {
    $('.buyBtn').click(function (e) {
        $.ajax({
            url: '../Store/AddToCart',
            type: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                id: e.currentTarget.id
            },
            success: function (data) {
                if ($('.cartCountIcon'.length == 0)){
                    let cartCountIcon = document.createElement('div');
                    let cartIcon = document.createElement('div');
                    let checkbox = document.createElement('input');
                    let img = document.createElement('img');
                    let cart = document.createElement('div');
                    cartCountIcon.className = "cartCountIcon";
                    cartIcon.className = "cartIcon";
                    checkbox.id = "cartCheck";
                    checkbox.type = "checkbox";
                    checkbox.setAttribute("hidden","hidden");
                    img.src = "../images/cart.png";
                    cart.className = "cart";
                    cart.style.width = "0px";
                    cart.style.height = "0px";
                    cart.style.display = "none";
                    document.body.append(cart);
                    document.body.append(cartIcon);
                    document.body.append(cartCountIcon);
                    cartIcon.append(img);
                    cartIcon.append(checkbox);
                }
                $('.cartCountIcon').html(data);
                $('.cartIcon').css({ 'margin-bottom': '5px' });
                $('.cartCountIcon').css({ 'margin-bottom': '5px' });
            }
        });
    })
});