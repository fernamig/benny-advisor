// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//
// Globals.
//

// TODO:Hard coded current term. Load it by ajax.
var gCurrentTermCode = 20191;
var gStudentId = null;
// TODO: this is a temporary workaround unti SSO support is added.
var gAdvisorId = null;

//
// Caching.
//

$.createCache = function(factory) {
    var cache = {};
    return function(key, force, callback) {
        if (force || !cache[key]) {
            cache[key] = $.Deferred(function(defer) {
                factory(defer, key);
            }).promise();
        }
        return cache[key].done(callback);
    };
}

$.cachedAjax = $.createCache(function(defer, url) {
    $.get(url).then(defer.resolve, defer.reject);
});

//
// Misc Helpers.
//

function getCoursesStats(courses) {
    var gpaGrade = 0;
    var gpaCredit = 0;
    var totalCredit = 0;
    $.each(courses, function(i, course) {
        if (course.grade >= 0) {
            gpaGrade += course.grade * course.credit;
            gpaCredit += course.credit;
        }
        totalCredit += course.credit;
    });

    var gpa = "";
    if (gpaCredit !== 0)
        gpa = (gpaGrade / gpaCredit).toFixed(1);

    return { gpa: gpa, credit: totalCredit };
}

function initTermCardLayout(data) {
    if (data.term.code < gCurrentTermCode)
        data.bgClass = "secondary";
    else if (data.term.code > gCurrentTermCode)
        data.bgClass = "primary";
    else
        data.bgClass = "success";

    var stats = getCoursesStats(data.courses);
    data.termCredit = stats.credit;
    data.termGpa = stats.gpa;
}

//
// Tab functionality.
//

function tabClick(e) {
    tabShow($(e).closest("[data-tab^='tab']").attr("data-tab"));
}

function tabShow(tabId) {
    // Call the initialize handler for the tab so that the data get loaded.
    var initHandler = tabId + "Init";
    eval("if (typeof " + initHandler + " !== 'undefined')" + initHandler + "()");

    // Hide all of the tab views.
    $(".tab-view").hide();

    // Show the tab view and select the tab side menu.
    $("#" + tabId).show();
    $(".nav-item").removeClass("active");
    $(".nav-item[data-tab=" + tabId + "]").addClass("active");
    // Set the url hash so that the url shows the active tab.
    window.location.hash = tabId;
}

//
// Student summary tab.
//

function tabSummaryInit() {
    var tmpl = $.templates("#termCardTmpl");

    // TODO: Load data by ajax get.
    var data = {
        bgClass: "bg-info",
        termCode: "20193",
        termTitle: "2019 Winter",
        courses: [{
            code: "X",
            title: "Sample Course",
            grade: "4.0",
            credit: 3
        },
        {
            code: "X2",
            title: "Another Course",
            grade: "4.0",
            credit: 3
        },
        {
            code: "X3",
            title: "One More Course",
            grade: "4.0",
            credit: 3
        }]
    };

    $("#tabSummaryContainer").html($(tmpl.render(data)));
}

//
// Course plan tab.
//

function tabCoursePlanInit() {
    var tmpl = $.templates("#coursePlanTmpl");

    $.cachedAjax("/api/ajax/GetAdvisingCoursePlan/" + gStudentId)
    .done(function(data) {
        $("#tabCoursePlanContainer").html($(tmpl.render(data)));
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}


//
// Timeline tab.
//

function tabTimelineInit() {
    var tmpl = $.templates("#timelineTmpl");

    $.cachedAjax("/api/ajax/GetAdvisingTimelime/" + gStudentId)
    .done(function(data) {
        $("#tabTimelineContainer").html($(tmpl.render(data)));
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Student progress tab.
//

function tabProgressInit() {
    var tmpl = $.templates("#progressTmpl");

    $.cachedAjax("/api/ajax/GetAdvisingProgress/" + gStudentId)
    .done(function(data) {
        $("#tabProgressContainer").html($(tmpl.render(data)));
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Make Appointment tab functionality.
//

function tabAppointmentInit() {
    // Get the start of the week.
    var wk = moment($("#tabAppointmentStart").attr("data-date"), "MM-DD-YYYY");

    $.cachedAjax("/api/ajax/GetAdvisingAvailability/" + gStudentId + "/" + gAdvisorId + "/" + wk.format("MM-DD-YYYY"))
    .done(function(data) {
        $.each(data, function(i, day) {
            $("#tabAppointmentContainer .row.card-header > div:nth-child(" + (2 + i) + ") span").html(moment(day.date).format("MMM DD"));

            $.each(day.slots, function(j, status) {
                var slot = $("#tabAppointmentContainer .row:nth-child(" + (2 + j) + ") > div:nth-child(" + (2 + i) + ")");
                slot.removeClass("text-muted")
                    .removeClass("alert-success")
                    .removeClass("alert-secondary")
                    .removeClass("alert-warning")
                    .removeClass("alert-danger");

                if (status === 0) {
                    slot.html("<a href='#'>Make Appt</a>");
                    slot.addClass("alert-success");
                }
                else if (status === 1) {
                    slot.html("Unavailable");
                    slot.addClass("text-muted");
                    slot.addClass("alert-secondary");
                }
                else if (status === 2) {
                    slot.html("Busy");
                    slot.addClass("text-muted");
                    slot.addClass("alert-secondary");
                }
                else if (status === 3) {
                    slot.html("Advising");
                    slot.addClass("text-muted");
                    slot.addClass("alert-warning");
                }
                else if (status === 4) {
                    slot.html("In Class");
                    slot.addClass("text-muted");
                    slot.addClass("alert-danger");
                }
            });
        });
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Notes tab.
//

function tabNotesInit() {
    var tmpl = $.templates("#notesTmpl");

    // TODO: Load data by ajax get.
    var data = [{
        context: null,
        note: "This is a sample note.",
        source: "Benny Advisor",
    },
    {
        context: "CS 101",
        note: "This is a sample note.",
        source: "Benny Advisor",
    },
    {
        context: null,
        note: "This is a sample note.",
        source: "Benny Advisor",
    }];

    $("#tabNotesContainer").html(tmpl.render(data));
}

//
// Test Scores tab.
//

function tabScoresInit() {
    var tmpl = $.templates("#scoresTmpl");
    
    // TODO: Load data by ajax get.
    var data = {
        fileDate: moment().format("MMM D, YYYY"),
        scores: 
        [{
            description: "SAT",
            score: "100",
            takenDate: moment().format("MMM D, YYYY"),
        },
        {
            description: "ACT",
            score: "100",
            takenDate: moment().format("MMM D, YYYY"),
        },
        {
            description: "GRE",
            score: "100",
            takenDate: moment().format("MMM D, YYYY"),
        }]
    };

    $("#tabScoresContainer").html($(tmpl.render(data)));
}

//
// Make appointment functionality.
//

function makeAppointmentShowPrevWeek() {
    var wk = moment($("#tabAppointmentStart").attr("data-date"), "MM-DD-YYYY");
    makeAppointmentSetDates(wk.subtract(7, 'days'));
    tabAppointmentInit();
}
function makeAppointmentShowNextWeek() {
    var wk = moment($("#tabAppointmentStart").attr("data-date"), "MM-DD-YYYY");
    makeAppointmentSetDates(wk.add(7, 'days'));
    tabAppointmentInit();
}
function makeAppointmentSetDates(wk) {
    var curr = moment().startOf("isoWeek");
    var last = moment().add(3, 'weeks').startOf("isoWeek");
    if (!wk)
        wk = curr;

    $("#tabAppointmentPrev").prop('disabled', curr >= wk);
    $("#tabAppointmentNext").prop('disabled', wk >= last);
    $("#tabAppointmentStart").attr("data-date", wk.format("MM-DD-YYYY"));
    $("#tabAppointmentStart").html(wk.format("MMM DD"));
    $("#tabAppointmentEnd").html(wk.add(4, 'days').format("MMM DD"));
}

