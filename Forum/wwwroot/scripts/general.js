﻿
let axiosConfig = '';
let axiosInstance;

axiosInstance = axios.create({
    baseURL: '',
    headers: {
        'Content-Type': 'application/json; charset=utf-8',
        //'Authorization': `Bearer ${token}`
    }
});

let summernoteToolbar = [
    ['style', ['style']],
    ['font', ['bold', 'underline', 'strikethrough', 'italic', 'clear', 'superscript', 'subscript', 'fontsize', 'fontname']],
    ['color', ['color']],
    ['para', ['ul', 'ol', 'paragraph']], //'height', 'style'
    ['insert', ['link', 'picture', 'table']],  //'video', 'hr'
    ['view', ['fullscreen']] //, 'codeview', 'help', 'undo', 'redo'
];

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


function formatDate(dt) {
    var d = new Date(dt);
    return d.toDateString();
}

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
