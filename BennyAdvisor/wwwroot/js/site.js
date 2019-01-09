// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.



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
    
    // TODO: Load data by ajax get.
    var data = [{
        bgClass: "bg-info",
        termCode: "20193",
        termTitle: "2019 Winter",
        termGpa: "3.3",
        termCredit: "12",
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
    },
    {
        bgClass: "bg-primary",
        termCode: "20193",
        termTitle: "2019 Winter",
        termGpa: "3.3",
        termCredit: "12",
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
    },
    {
        bgClass: "bg-secondary",
        termCode: "20193",
        termTitle: "2019 Winter",
        termGpa: "3.3",
        termCredit: "12",
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
    }];

    $("#tabCoursePlanContainer").html($(tmpl.render(data)));
}


//
// Timeline tab.
//

function tabTimelineInit() {
    var tmpl = $.templates("#timelineTmpl");
    
    // TODO: Load data by ajax get.
    var data = [
    {
        title: "Fall 2018 to Spring 2019",
        terms: [{
            bgClass: "bg-info",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        },
        {
            bgClass: "bg-primary",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        },
        {
            bgClass: "bg-secondary",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        }
        ]
    },
    {
        title: "Fall 2018 to Spring 2019",
        terms: [{
            bgClass: "bg-info",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        },
        {
            bgClass: "bg-primary",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        },
        {
            bgClass: "bg-secondary",
            termCode: "20193",
            termTitle: "2019 Winter",
            termGpa: "3.3",
            termCredit: "12",
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
        }
        ]
    }
    ];

    $("#tabTimelineContainer").html($(tmpl.render(data)));
}

//
// Student progress tab.
//

function tabProgressInit() {
    var tmpl = $.templates("#progressTmpl");

    // TODO: Load data by ajax get.
    var data = [
    {
        title: "Requirements part 1",
        bgClass: "bg-danger",
        credit: 9,
        requirements: [{
            bgClass: "alert-danger",
            iconClass: "far fa-square",
            title: "A requirement",
            credit: 3,
            status: 1,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-primary",
            iconClass: "fas fa-minus-square",
            title: "A requirement",
            credit: 3,
            status: 0,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-success",
            iconClass: "fas fa-check-square",
            title: "A requirement",
            credit: 3,
            status: 2,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        }]
    },
        {
        title: "Requirements part 1",
        bgClass: "bg-primary",
        credit: 9,
        requirements: [{
            bgClass: "alert-danger",
            iconClass: "far fa-square",
            title: "A requirement",
            credit: 3,
            status: 1,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-primary",
            iconClass: "fas fa-minus-square",
            title: "A requirement",
            credit: 3,
            status: 0,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-success",
            iconClass: "fas fa-check-square",
            title: "A requirement",
            credit: 3,
            status: 2,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        }]
    },
    {
        title: "Requirements part 1",
        bgClass: "bg-success",
        credit: 9,
        requirements: [{
            bgClass: "alert-danger",
            iconClass: "far fa-square",
            title: "A requirement",
            credit: 3,
            status: 1,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-primary",
            iconClass: "fas fa-minus-square",
            title: "A requirement",
            credit: 3,
            status: 0,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        },
        {
            bgClass: "alert-success",
            iconClass: "fas fa-check-square",
            title: "A requirement",
            credit: 3,
            status: 2,
            course: {
                code: "X",
                title: "Sample",
                grade: "4.0",
                term: "Winter 2019"
            }
        }]
    }
    ];


    $("#tabProgressContainer").html($(tmpl.render(data)));
}

//
// Make Appointment tab functionality.
//

function tabAppointmentInit() {
    // TODO: Load data by ajax get.
    var day = moment().startOf("isoWeek");

    $("#tabAppointmentStart").html(day.format("MMM D"));
    $("#tabAppointmentEnd").html(moment(day).add(4, "days").format("MMM D"));

    for (var i = 0; i < 5; i++) {
        // Set the date in the day header.
        $("#tabAppointmentContainer .row.card-header > div:nth-child(" + (2 + i) + ") span").html(day.format("MMM D"));
        day.add(1, "days");

        for (var j = 0; j < 4; j++) {
            var slot = $("#tabAppointmentContainer .row:nth-child(" + (2 + j) + ") > div:nth-child(" + (2 + i) + ")");
            slot.html("<a href='#'>Make Appt</a>");
            slot.addClass("alert-success");
        }
        for (var j = 4; j < 8; j++) {
            var slot = $("#tabAppointmentContainer .row:nth-child(" + (2 + j) + ") > div:nth-child(" + (2 + i) + ")");
            slot.html("Unavailable");
            slot.addClass("text-muted");
            slot.addClass("alert-secondary");
        }
        for (var j = 8; j < 12; j++) {
            var slot = $("#tabAppointmentContainer .row:nth-child(" + (2 + j) + ") > div:nth-child(" + (2 + i) + ")");
            slot.html("Reserved");
            slot.addClass("text-muted");
            slot.addClass("alert-secondary");
        }
        for (var j = 12; j < 16; j++) {
            var slot = $("#tabAppointmentContainer .row:nth-child(" + (2 + j) + ") > div:nth-child(" + (2 + i) + ")");
            slot.html("In Class");
            slot.addClass("text-muted");
            slot.addClass("alert-danger");
        }
    }
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

