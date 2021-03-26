console.log("succes");
$.get('recommender/GetSuggestions/', function (data) {

    for (var m = 550; m < 800; m++) {
        let hnr = new XMLHttpRequest();
        hnr.onload = function () {
            if (hnr.status >= 200 && hnr.status < 300) {
                let resultParsed = JSON.parse(hnr.response);
                console.log("ii");
                CreateMoviePoster("movies", resultParsed);

                movieRepeat:
                for (var n = 0; n < data.length; n++) {
                    var genreOne = resultParsed.genres[0].name;
                    var genreTwo = resultParsed.genres[1].name;
                    for (var x = 0; x < jQuery("#recommended").children().length; x++) {
                        var divChildren = jQuery("#recommended").children()[x]
                        if (divChildren != null) {
                            var lastDivElement = divChildren.lastElementChild;
                            if (lastDivElement != null) {
                                if (lastDivElement.getAttribute("src") == "https://image.tmdb.org/t/p/w200" + resultParsed.poster_path == true) {
                                    break movieRepeat;
                                }
                            }
                        }
                    }
                    if (data[n].genre.includes(genreOne) || data[n].genre.includes(genreTwo)) {
                        if (resultParsed.vote_average > 6) {

                            CreateMoviePoster("recommended", resultParsed);
                        }
                   }
                }


                document.getElementsByClassName("lds-roller")[0].style.display = "none";
                document.getElementsByClassName("h3")[0].style.visibility = "visible";
                document.getElementsByClassName("h3")[1].style.visibility = "visible";
                Console.log("dd");
                return resultParsed;
            } else {
                console.log("fail");
            }
        }
        hnr.open("GET", "https://api.themoviedb.org/3/movie/" + m + "?api_key=c23a9154c89a0d00616d6d0d2544c976");
        hnr.send();
    }
}, 'json');
function CreateMoviePoster(divId, resultParsed) {
    var img = document.createElement("img");
    img.setAttribute("src", "https://image.tmdb.org/t/p/w200" + resultParsed.poster_path);
    var elementA = document.createElement("a");
    var genres = "";
    for (var i = 0; i < resultParsed.genres.length; i++) {
        genres += resultParsed.genres[i].name + " ";
    }
    elementA.setAttribute("id", "a");
    elementA.style.padding = "5px";
    elementA.setAttribute("href", "movie/CreateViewAction/" + resultParsed.original_title + "/" + genres + "/" + resultParsed.id);
    elementA.appendChild(img);
    document.getElementById(divId).appendChild(elementA);
}
