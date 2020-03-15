
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
