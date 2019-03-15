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

$.fn.disable = function() {
    return this.each(function() {
        if (typeof this.disabled != "undefined") this.disabled = true;
    });
}

$.fn.enable = function() {
    return this.each(function() {
        if (typeof this.disabled != "undefined") this.disabled = false;
    });
}

//
// Misc Helpers.
//

function findFirst(array, predicate) {
    for (var i = 0; i < array.length; i++)
        if (predicate(array[i]))
            return array[i];
    return null;
}

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

    return { gpa: gpa, credit: totalCredit, count: courses.length };
}

function initTermCardLayout(data) {
    if (data.term.code < gCurrentTermCode)
        data.bgClass = "info";
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
    $.cachedAjax("/api/ajax/GetAdvisingCoursePlan/" + gStudentId)
    .done(function(data) {
        if (!data || data.length === 0) {
            var tmpl = $.templates("#infoMessageTmpl");
            $("#tabCoursePlanContainer").html($(tmpl.render("A course plan has not been created.")));
        }
        else {
            var tmpl = $.templates("#coursePlanTmpl");
            $("#tabCoursePlanContainer").html($(tmpl.render(data)));
        }
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

function tabAppointmentInit(wk) {
    // Get the start of the week.
    if (!wk)
        wk = $("#tabAppointmentStart").attr("data-date");

    $.get("/api/ajax/GetAdvisingAvailability/" + gStudentId + "/" + gAdvisorId + "/" + wk)
    .done(function(data) {
        if (data.timeTrade) {
            var tmpl = $.templates("#scheduleTimeTradeTmpl");
            $("#tabAppointmentContainer").html(tmpl.render(data.timeTrade));
        }
        else if (data.builtin) {
            var tmpl = $.templates("#scheduleBuiltinTmpl");
            $("#tabAppointmentContainer").html(tmpl.render(data.builtin));
        }
        else
        {
            var tmpl = $.templates("#scheduleNoneTmpl");
            $("#tabAppointmentContainer").html(tmpl.render({ service: gAdvisorId }));
        }
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Notes tab.
//

function tabNotesInit() {
    $.cachedAjax("/api/ajax/GetStudentNotes/" + gStudentId)
    .done(notesShowNotes)
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Test Scores tab.
//

function tabScoresInit() {
    $.cachedAjax("/api/ajax/GetStudentTestScores/" + gStudentId)
    .done(function(data) {
        if (!data || data.length === 0) {
            var tmpl = $.templates("#infoMessageTmpl");
            $("#tabScoresContainer").html($(tmpl.render("No test scores have been submitted.")));
        }
        else {
            var tmpl = $.templates("#scoresTmpl");
            $("#tabScoresContainer").html($(tmpl.render(data)));
        }
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Scheduler functionality.
//

function schedulerShowPrevWeek() {
    var wk = moment($("#tabAppointmentStart").attr("data-date"), "MM-DD-YYYY");
    $("#tabAppointmentStart").attr("data-date", wk.subtract(7, 'days').format("MM-DD-YYYY"));
    tabAppointmentInit();
}

function schedulerShowNextWeek() {
    var wk = moment($("#tabAppointmentStart").attr("data-date"), "MM-DD-YYYY");
    $("#tabAppointmentStart").attr("data-date", wk.add(7, 'days').format("MM-DD-YYYY"));
    tabAppointmentInit();
}

function schedulerSelectAdvisor(el) {
    $("#tabAppointmentContainer").html($("#spinnerContainer").html());

    gAdvisorId = $(el).attr("data-id");
    $("#selectedAdvisor").attr("data-id", gAdvisorId);
    $("#selectedAdvisor").html($(el).html());

    var curr = moment().add(2, 'days').startOf("isoWeek");
    var currFmt = curr.format("MM-DD-YYYY");
    $("#tabAppointmentStart").attr("data-date", currFmt);
    tabAppointmentInit(currFmt);
}

// TODO: Merge schedulerSelectService with schedulerSelectAdvisor.
function schedulerSelectService() {
    $("#tabAppointmentContainer").html($("#spinnerContainer").html());

    var curr = moment().add(2, 'days').startOf("isoWeek");
    var currFmt = curr.format("MM-DD-YYYY");
    $("#tabAppointmentStart").attr("data-date", currFmt);
    tabAppointmentInit(currFmt);
}

function schedulerShowCreateAppointmentModal() {
    $('#schedulerCreateAppointmentModal').modal('show');
}

function schedulerSaveDay(day) {
    schedulerSetDay(day);
    schedulerSendUpdate();
}

function schedulerSetDay(day) {
    var val = $("#" + day + " > textarea").val();
    var parts = val.split(",");
    var text = [];
    var badges = [];
    parts.forEach(function (el) {
        var range = el.split("-");
        if (range.length === 2) {
            var startTime = moment(range[0], ['h:m a', 'h:ma', 'ha']);
            var endTime = moment(range[1], ['h:m a', 'h:ma', 'ha']);
            if (startTime < endTime) {
                var start = startTime.format('h:mma');
                var end = endTime.format('h:mma');
                text.push(start + "-" + end);
                badges.push(
                    '<span class="badge badge-warning">'
                    + start + " - " + end +
                    '</span>');
            }
        }
    });

    $("#" + day + " > textarea").val(text.join(", "));
    $("#" + day + " > div").html(badges.join(" "));
}

function schedulerSendUpdate() {
    var type = $('input[name=scheduler]:checked').val();
    var data = { };

    if (type === "timetrade") {
        data.timetrade = { };
        data.timetrade.url = $("#timetradeUrl").val();
    }
    else if (type === "builtin") {
        data.builtin = { };
        data.builtin.limits = {
            minHours: $("#minHours").val(),
            maxDays: $("#maxDays").val(),
            appointmentLength: $("output").text()
        };
        data.builtin.availability = {
            monday: $("#dayMonday > textarea").val(),
            tuesday: $("#dayTuesday > textarea").val(),
            wednesday: $("#dayWednesday > textarea").val(),
            thursday: $("#dayThursday > textarea").val(),
            friday: $("#dayFriday > textarea").val()
        };
    }

    $.ajax({
        type: "POST",
        url: "/api/ajax/MyProfileSetSchedule/" + gAdvisorId,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
    .fail(function (xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

function schedulerInit(data, slider) {
    if (data.timeTrade) {
        $("#timetradeUrl").val(data.timeTrade.url);
        $("input[name=scheduler][value=timetrade").prop("checked", true);
        schedulerSetDefaultBuiltin();
        schedulerSelectTimeTrade();
    }
    else if (data.builtin) {
        $("input[name=scheduler][value=builtin").prop("checked", true);
        schedulerInitBuiltin(data.builtin);
        schedulerSelectBuiltin();
    }
    else {
        // There is not schedule preferences so set the defaults for the built-in.
        schedulerSetDefaultBuiltin();
    }
    $("#scheduleChooseContainer").removeClass("d-none");
    $("#spinner").hide();
}

function schedulerSetDefaultBuiltin() {
    schedulerInitBuiltin({
        availability: {
            monday: "",
            tuesday: "",
            wednesday: "",
            thursday: "",
            friday: ""
        },
        limits: {
            minHours: 8,
            maxDays: 21,
            appointmentLength: 30
        }
    });
}

function schedulerInitBuiltin(data) {
    $("#dayMonday > textarea").val(data.availability.monday);
    schedulerSetDay("dayMonday");
    $("#dayTuesday > textarea").val(data.availability.tuesday);
    schedulerSetDay("dayTuesday");
    $("#dayWednesday > textarea").val(data.availability.wednesday);
    schedulerSetDay("dayWednesday");
    $("#dayThursday > textarea").val(data.availability.thursday);
    schedulerSetDay("dayThursday");
    $("#dayFriday > textarea").val(data.availability.friday);
    schedulerSetDay("dayFriday");

    $("#minHours").val(data.limits.minHours);
    $("#maxDays").val(data.limits.maxDays);
    $("output").val(data.limits.appointmentLength);
    $("#sliderApptLen").slider("setValue", data.limits.appointmentLength);
}

function schedulerSelectBuiltin() {
    $("#timetradeUrl").attr("disabled", "disabled");
    $("#builtinContainer").removeClass("d-none");
    $("#sliderApptLen").slider("relayout");
    $("#sliderApptLen").slider("refresh");
}

function schedulerSelectTimeTrade() {
    $("#timetradeUrl").removeAttr("disabled");
    $("#builtinContainer").addClass("d-none");
}

//
// Add note functionality.
//

function notesShowNotes(data) {
    if (!data || data.length === 0) {
        var tmpl = $.templates("#infoMessageTmpl");
        $("#tabNotesContainer").html($(tmpl.render("No notes have been added.")));
    }
    else {
        var tmpl = $.templates("#notesTmpl");
        $("#tabNotesContainer").html($(tmpl.render(data)));
    }
}

function notesShowAddModal() {
    $("#notesAddNote").val("");
    $('#notesAddModal').modal('show');
}

function notesAdd() {
    $('#notesAddModal').modal('hide');

    var note = {
        context: null,
        note: $("#notesAddNote").val()
    };

    $.ajax({
        type: "POST",
        url: "/api/ajax/AddStudentNote/" + gStudentId,
        data: JSON.stringify(note),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
    .done(function(data) {
        $.cachedAjax("/api/ajax/GetStudentNotes/" + gStudentId, true)
        .done(notesShowNotes)
        .fail(function(xhr, status, error) {
            alert("Request Failed: " + status + ", " + error);
        });
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

//
// Course plan functionality.
//

function coursePlanExpandAllCards(expand)
{
    $.each($("#tabCoursePlanContainer > .card"), function() {
        if (expand)
            $(this).removeClass("show-summary");
        else
            $(this).addClass("show-summary");
    });
}

function coursePlanSendUpdate() {
    var plans = [];

    $("#tabCoursePlanContainer > .card").each(function() {
        var courseIds = [];
        $(this).find(".row.course").each(function() {
            courseIds.push($(this).attr("data-id"));
        });

        plans.push({
            "id": $(this).attr("data-id"),
            "title": $(this).attr("data-title"),
            "members": courseIds,
        });
    });

    $.ajax({
        type: "POST",
        url: "/api/ajax/SetCoursePlan/" + gStudentId,
        data: JSON.stringify(plans),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

function coursePlanGetCourseCardStats(card) {
    var courses = [];
    card.find(".row.course").each(function() {
        courses.push({
            "grade": parseFloat($(this).attr("data-grade")),
            "credit": parseInt($(this).attr("data-credit")),
        });
    });
    return getCoursesStats(courses);
}

function coursePlanUpdateTermStats(card) {
    var stats = coursePlanGetCourseCardStats(card);
    card.find(".termGpa").text(stats.gpa);
    card.find(".termCredit").text(stats.credit);
    card.find(".termCourseCount").text(stats.count);
}

function coursePlanAddCourseToTerm(courseCode)
{
    var termCode = $("#addCourseTermCode").val();
    $("#addCourseModal").modal("hide");

    $.cachedAjax("/api/ajax/GetCourseData/" + courseCode)
    .done(function(data) {
        var tmpl = $.templates("#coursePlanEntryTmpl");

        data.status = 2;
        data.grade = -1;
        $("#coursePlanTerm" + termCode).append($(tmpl.render(data)));
        coursePlanUpdateTermStats($("#coursePlanTerm" + termCode).closest(".card"));
        coursePlanSendUpdate();
    })
    .fail(function(xhr, status, error) {
        alert("Request Failed: " + status + ", " + error);
    });
}

function coursePlanDisableSelectedTerms() {
    $(".add-term").removeAttr("disabled");
    $(".card.term").each(function () {
        var termCode = $(this).attr("data-id");
        $(".add-term[data-code=" + termCode + "]").attr("disabled", "disabled");
    });
}

function coursePlanShowAddCourseModal(termCode) {
    $("#courseFilter").val('');
    $("#courseList").html('<li class="list-group-item">Loading...</li>');
    $('#addCourseTermCode').val(termCode);
    $('#addCourseModal').modal('show');

    $.cachedAjax("/api/ajax/GetCourses")
        .done(function (data) {
            var tmpl = $.templates("#searchCourseItemTmpl");
            $("#courseList").html(tmpl.render(data));
        })
        .fail(function (xhr, status, error) {
            alert("Request Failed: " + status + ", " + error);
        });
}

function coursePlanShowRemoveTermModal(termCode, termTitle) {
    $('#removeTermCode').val(termCode);
    $('#removeTermTitle').text(termTitle);
    $('#removeTermModal').modal('show');
}

function coursePlanOnRemoveTerm() {
    var termCode = $("#removeTermCode").val();
    $("#coursePlanTerm" + termCode).closest(".card").remove();
    $("#removeTermModal").modal("hide");
    coursePlanSendUpdate();
}

function coursePlanShowRemoveCourseModal(courseCode, courseTitle, termCode, termTitle) {
    $('#removeCourseCode').val(courseCode);
    $('#removeCourseTitle').text(courseTitle);
    $('#removeCourseTermCode').val(termCode);
    $('#removeCourseTermTitle').text(termTitle);
    $('#removeCourseModal').modal('show');
}

function coursePlanOnRemoveCourse() {
    var courseCode = $("#removeCourseCode").val();
    var termCode = $("#removeCourseTermCode").val();
    $("#coursePlanTerm" + termCode).find(".course[data-id=" + courseCode + "]").remove();
    $("#removeCourseModal").modal("hide");
    coursePlanSendUpdate();
}
