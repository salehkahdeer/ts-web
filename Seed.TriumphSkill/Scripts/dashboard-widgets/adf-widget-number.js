(function(window, undefined) {'use strict';
/*
 * The MIT License
 *
 * Copyright (c) 2015, Sebastian Sdorra
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */



angular.module('adf.widget.number', ['adf.provider'])
  .config(["dashboardProvider", function(dashboardProvider){
    dashboardProvider
      .widget('number', {
        title: 'Number',
        description: 'Displays a key performance indicator',
        templateUrl: '{widgetsPath}/number/src/view.html',
          frameless: true,
        controller: 'numberController',
        controllerAs: 'number',
        config: {
            background: '#d9534f',
            foreground: '#ffffff',
            icon: 'glyphicon-tasks',
            description: 'Total Sales',
            url: 'http://localhost:55952/Village/Statistics'
        },
        edit: {
          templateUrl: '{widgetsPath}/number/src/edit.html'
        }
      });
  }])
  .controller('numberController', ["$scope", "$http", "config", function($scope, $http, config){
    var number = this;

    function setNumber(){
        
        number.background = config.background;
        number.foreground = config.foreground;
        number.icon = config.icon;
        number.description = config.description;
        number.url = config.url;
        number.kpi = 0;
        number.link = '';
        $http.get(config.url).
        success(function (data) {
            number.kpi = data.Kpi;
            number.link = data.Link;
        });
    }

    setNumber();
    
  }]);

angular.module("adf.widget.number").run(["$templateCache", function ($templateCache) {
    $templateCache.put("{widgetsPath}/number/src/edit.html", "<form role=form><div class=form-group><label for=url>Url</label> <input type=text class=form-control id=url ng-model=config.url></div><div class=form-group><label for=icon>Icon</label> <input type=text class=form-control id=icon ng-model=config.icon></div><div class=form-group><label for=description>Description</label> <input type=text class=form-control id=description ng-model=config.description></div><div class=form-group><label for=background>Background</label> <input type=text class=form-control id=background ng-model=config.background></div><div class=form-group><label for=foreground>Foreground</label> <input type=text class=form-control id=foreground ng-model=config.foreground></div></form>");
    $templateCache.put("{widgetsPath}/number/src/view.html", "<div class=number style='background: {{number.background}}; color: {{number.foreground}}; overflow: auto; border-radius: 4px;'><div class='number-icon'><span class='glyphicon {{number.icon}}' aria-hidden='true'></span></div><div class='number-text'><div class=number-kpi>{{number.kpi}}</div><div class=number-description>{{number.description}}</div></div><div class=number-details style='background: {{number.foreground}}; overflow: auto; border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; clear: both; margin: 1px; '><a href='{{number.link}}' style='color: {{number.background}};'><div class='number-link-text'>View Details</div><div class='number-link-icon'><span class='glyphicon glyphicon-circle-arrow-right'></span></div></a></div></div>");
}]);
})(window);