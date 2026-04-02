(function () {
    function isProtectedApiRequest(url) {
        if (!url) {
            return false;
        }

        try {
            url = new URL(url, window.location.origin).pathname;
        } catch (error) {
            return false;
        }

        return url.startsWith("/api/Emp") || url.startsWith("/api/Admin");
    }

    function getToken() {
        return sessionStorage.getItem("jwt");
    }

    function clearAuth() {
        sessionStorage.removeItem("jwt");
    }

    function redirectToLogin() {
        if (window.location.pathname !== "/AuthenticationUI/Login"
            && window.location.pathname !== "/AuthenticationUI/Logout") {
            window.location.href = "/AuthenticationUI/Logout";
        }
    }

    window.appAuth = {
        clearAuth: clearAuth,
        getToken: getToken,
        redirectToLogin: redirectToLogin
    };

    $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
        if (!isProtectedApiRequest(options.url)) {
            return;
        }

        var originalBeforeSend = options.beforeSend;
        options.beforeSend = function (xhr, settings) {
            var token = getToken();
            if (!token) {
                redirectToLogin();
                return false;
            }

            xhr.setRequestHeader("Authorization", "Bearer " + token);

            if (typeof originalBeforeSend === "function") {
                return originalBeforeSend.call(this, xhr, settings);
            }
        };
    });

    $(document).ajaxError(function (event, jqXHR, ajaxSettings) {
        var requestUrl = ajaxSettings && ajaxSettings.url;
        if (jqXHR.status === 401 && isProtectedApiRequest(requestUrl)) {
            clearAuth();
            redirectToLogin();
        }
    });

    $(function () {
        if (window.location.pathname.startsWith("/EmployeeUI") && !getToken()) {
            redirectToLogin();
            return;
        }

        $("#logoutLink").on("click", function () {
            clearAuth();
        });
    });
})();
