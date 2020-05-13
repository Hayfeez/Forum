
let axiosConfig = '';
let axiosInstance;

axiosInstance = axios.create({
    baseURL: '',
    headers: {
        'Content-Type': 'application/json; charset=utf-8',
        //'Authorization': `Bearer ${token}`
    }
});


let STATUS = {
    Success:1,
    Failed:2,
    NotFound:3,
    Error:4
};


let UserActionEnum = {
    Bookmark: 1,
    Flag:2,
    Follow:3,
    Like:4,
    Share:5,
    Upvote:6,
    Downvote:7
};

function buildDropdownOption(text, value) {
    var option = document.createElement("option");
    option.text = text;
    option.value = value;

    return option;
}

function appendToDropdown(dropdownId, options) {
    var select = document.getElementById(dropdownId);
    select.appendChild(options);
}
