function d(m) {
    console.log("succes");
     let hnr = new XMLHttpRequest();
     hnr.onload =  function () {
        if (hnr.status >= 200 && hnr.status < 300) {
            console.log("succes");
            let resultParsed = JSON.parse(hnr.response);
            console.log(resultParsed);
            document.getElementById("poster").setAttribute("src", "https://image.tmdb.org/t/p/w200" + resultParsed.poster_path);
            var div = document.getElementById("details");
            var elementH2One = document.createElement("h2");
            elementH2One.innerText = "Movie Details";
            elementH2One.style.color = "#FF8C00";
            var elementPOne = document.createElement("p");
            elementPOne.innerText = resultParsed.overview;
            elementPOne.style.color = "#FF8C00";
            div.appendChild(elementH2One);
            div.appendChild(elementPOne);

            return resultParsed;
        } else {
            console.log("fail")
        }
    }
    hnr.open("GET", "https://api.themoviedb.org/3/movie/" + m + "?api_key=c23a9154c89a0d00616d6d0d2544c976");
    hnr.send();
}