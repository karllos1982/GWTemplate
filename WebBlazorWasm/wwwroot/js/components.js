
window.cookieFunctions = {

    createCookie: function (name, value, expires) {
        
        var d = new Date(expires);
        var expires = "expires=" + d.toUTCString();

        document.cookie = name + "=" + value + ";" + expires + "; path=/";        
       
    }
    ,

    getCookie: function (cname) {        
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        
        return "";
    }
    ,

    removeCookie: function(name) {
        document.cookie = name + '=; Max-Age=-99999999;';
    }
    
}

window.menuFunctions = {

    showMenu: function (ctrl) {

        
        var element = document.getElementById(ctrl);
       
        element.classList.add("active").
        element.classList.add("show");

    }
    ,

    hideMenu: function (ctrl) {        

        var element = document.getElementById(ctrl);

        console.log(element); 

        element.classList.remove("active").
        element.classList.remove("show");
    }

}

    


