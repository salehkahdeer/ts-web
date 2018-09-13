var app = angular.module('Dashboard', ['adf', 'adf.widget.clock', 'adf.widget.linklist', 'adf.widget.markdown', 'adf.widget.number'])
.config(function (dashboardProvider) {
    dashboardProvider
      .structure('6-6', {
          rows: [{
              columns: [{
                  styleClass: 'col-md-6'
              }, {
                  styleClass: 'col-md-6'
              }]
          }]
      });
});

app.controller('dashboardController', function ($scope, $http, $attrs) {    

    $http.get("http://localhost:55952/Home/Dashboard?name=" + $attrs.name)
    .then(function success(response) {        
        $scope.model = response.data;
        //if null you can set a default. as shown in the commented model below.

    }, function error(response) {

        //handle error message

    });

    //var model = {
    //    title: "Dashboard",
    //    structure: "6-6",
    //    rows: [{
    //        columns: [{
    //            styleClass: "col-md-3",
    //            widgets: [{
    //                type: "number",
    //                config: {
    //                    background: '#337ab7',
    //                    foreground: '#ffffff',
    //                    icon: 'glyphicon-tasks',
    //                    description: 'Total Sales',
    //                    url: 'http://localhost:55952/Village/Statistics'
    //                },
    //                title: "Total Sales"
    //            }]
    //        }, {
    //            styleClass: "col-md-3",
    //            widgets: [{
    //                type: "number",
    //                config: {
    //                    background: '#5cb85c',
    //                    foreground: '#ffffff',
    //                    icon: 'glyphicon-usd',
    //                    description: 'Total Sales',
    //                    url: 'http://localhost:55952/Village/Statistics'
    //                },
    //                title: "Number Widget"
    //            }]
    //        },
    //        {
    //            styleClass: "col-md-3",
    //            widgets: [{
    //                type: "number",
    //                config: {
    //                    background: '#f0ad4e',
    //                    foreground: '#ffffff',
    //                    icon: 'glyphicon-shopping-cart',
    //                    description: 'Total Sales',
    //                    url: 'http://localhost:55952/Village/Statistics'
    //                },
    //                title: "Total Sales"
    //            }]
    //        }, {
    //            styleClass: "col-md-3",
    //            widgets: [{
    //                type: "number",
    //                config: {
    //                    background: '#d9534f',
    //                    foreground: '#ffffff',
    //                    icon: 'glyphicon-bell',
    //                    description: 'Total Sales',
    //                    url: 'http://localhost:55952/Village/Statistics'
    //                },
    //                title: "Number Widget"
    //            }]
    //        }]
    //    },
    //    {
    //        columns: [{
    //            styleClass: "col-md-6",
    //            widgets: [{
    //                type: "markdown",
    //                config: {
    //                    content: "![celusion logo](http://www.celusion.com/wp-content/uploads/2012/04/logo.png)   \n \n \n We are a team of highly skilled software professionals who are passionate about technology and committed to deliver in a challenging environment. We build software using an agile approach, that helps keep up with business dynamics. We empower our team to stay ahead of the technology curve and apply new & upcoming trends to solve business problems. \n \n \n Our engineering team is responsible for the research and development of pioneering software products in the field of mobility & communication. Over 1500 organizations across the globe, leverage these products integrated with their core business systems. Our current product portfolio comprise of SMS ConneXion® – Enterprise Messaging Platform & Mobiliteam™ – Enterprise Mobility Platform. \n \n \n Our services team provide the design, technology, team and methodology to deliver complex business software for Enterprises. We build desktop, web & mobile applications and are unbiased towards the use of different technologies to solve different problems. Our expertise, spans a broad range of technologies like .NET, Java, PHP, Android, iOS, HTML5 and JavaScript."
    //                },
    //                title: "Markdown"
    //            }
    //            ]
    //        }, {
    //            styleClass: "col-md-6",
    //            widgets: [{
    //                type: "clock",
    //                config: {
    //                    timePattern: "HH:mm:ss",
    //                    datePattern: "DD MMM YYYY"
    //                },
    //                title: "Digital Clock"
    //            },
    //            {
    //                type: "linklist",
    //                config: {
    //                    links: [{
    //                        title: "Celusion Technologies",
    //                        href: "http://www.celusion.com"
    //                    }, {
    //                        title: "SMS ConneXion",
    //                        href: "http://www.smsconnexion.com"
    //                    }, {
    //                        title: "Mobiliteam",
    //                        href: "http://www.mobiliteam.in"
    //                    }]
    //                },
    //                title: "Links"
    //            }]
    //        }]
    //    }]
    //};

    //$scope.model = model;

    $scope.$on('adfDashboardChanged', function (event, name, model) {
        $http.post("http://localhost:55952/Home/Dashboard?name=" + name, model);
    });

});