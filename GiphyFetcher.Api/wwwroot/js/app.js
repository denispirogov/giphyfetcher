const grid = document.getElementById("gifGrid");
const status = document.getElementById("status");

const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");
const trendingButton = document.getElementById("trendingButton");

async function loadGifs(url) {
    try {
        setLoading(true);

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error("Failed to load GIFs");
        }

        const gifs = await response.json();

        renderGifs(gifs);
    }
    catch (error) {
        console.error(error);

        status.textContent =
            "Failed to load GIFs. Please try again.";
    }
    finally {
        setLoading(false);
    }
}

function renderGifs(gifs) {
    grid.innerHTML = "";

    if (!gifs.length) {
        status.textContent = "No GIFs found.";
        return;
    }

    status.textContent = `${gifs.length} GIFs found`;

    for (const gif of gifs) {
        const image = document.createElement("img");

        image.src = gif.url;
        image.alt = "GIF";
        image.loading = "lazy";

        grid.appendChild(image);
    }
}

function setLoading(isLoading) {
    if (isLoading) {
        status.textContent = "Loading...";
    }
}

searchButton.addEventListener("click", () => {
    const term = searchInput.value.trim();

    if (!term) {
        return;
    }

    loadGifs(
        `/api/gifs/search?term=${encodeURIComponent(term)}`
    );
});

searchInput.addEventListener("keydown", event => {
    if (event.key === "Enter") {
        searchButton.click();
    }
});

trendingButton.addEventListener("click", () => {
    loadGifs("/api/gifs/trending");
});

loadGifs("/api/gifs/trending");