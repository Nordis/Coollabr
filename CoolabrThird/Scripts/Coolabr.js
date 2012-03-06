var Coollabr = Coollabr || { };

$(document).ready(function () {

    //Task View Model
    function taskViewModel(id, title, titleAsMd, completed, date, ownerViewModel) {
        this.id = id;
        this.title = ko.observable(title);
        this.titleAsMd = ko.observable(titleAsMd);
        this.completed = ko.observable(completed);
        this.date = ko.observable(date);
        this.remove = function () { ownerViewModel.removeTask(this.id); };
        this.complete = function () {
            var oldValue = this.completed();
            this.completed(!oldValue);
        };
        this.chngeText = function (sender) {
            Coollabr.Tasks.ToggleTask(sender);
        };
        this.edit = function (sender) {
            Coollabr.Tasks.ToggleTask(sender);
        };
        this.notification = function (b) { notify = b; };

        var self = this;

        this.title.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

        this.completed.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

    }

    function taskListViewModel() {
        this.hub = $.connection.tasks;
        this.tasks = ko.observableArray([]);
        this.newTaskText = ko.observable();
        var tasks = this.tasks;
        var self = this;
        var notify = true;

        //Initializes the view model
        this.init = function () {
            this.hub.getAllTasks();
        };

        this.hub.taskAll = function (allTasks) {

            var mappedTasks = $.map(allTasks, function (item) {
                return new taskViewModel(item.Id, item.Title, item.TitleAsMd,
                    item.Completed, item.LastUpdatedFormatted, self);
            });

            tasks(mappedTasks);
        };

        this.hub.taskUpdated = function (t) {
            var task = ko.utils.arrayFilter(tasks(), function (value) { return value.id == t.Id; })[0];
            notify = false;
            task.title(t.Title);
            task.titleAsMd(t.TitleAsMd);
            task.completed(t.Completed);
            task.date(t.LastUpdatedFormatted);
            notify = true;
        };

        this.hub.reportError = function (error) {
            hideMessages();
            $("#error").text(error);
            $("#error").fadeIn(1000, function () {
                $("#error").fadeOut(4000);
            });
        };

        this.hub.reportSuccess = function (message) {
            hideMessages();
            $("#success").text(message);
            $("#success").fadeIn(1000, function () {
                $("#success").fadeOut(4000);
            });
        };

        function hideMessages() {
            $("#success").stop().hide();
            $("#error").stop().hide();
        }

        this.hub.taskAdded = function (t) {
            tasks.unshift(new taskViewModel(t.Id, t.Title, t.TitleAsMd, t.Completed, t.LastUpdatedFormatted, self));
        };

        this.hub.taskRemoved = function (id) {
            var task = ko.utils.arrayFilter(tasks(), function (value) { return value.id == id; })[0];
            tasks.remove(task);
        };

        //View Model 'Commands'

        //To create a task
        this.addTask = function () {
            var t = { "title": this.newTaskText(), "completed": false };
            this.hub.add(t).done(function () {
                console.log('Success!');
            }).fail(function (e) {
                console.warn(e);
            });

            this.newTaskText("");
        };

        //To remove a task
        this.removeTask = function (id) {
            this.hub.remove(id);
        };

        //To update this task
        this.updateTask = function (task) {
            if (notify)
                this.hub.update(task);
        };

        //Gets the incomplete tasks
        this.incompleteTasks = ko.dependentObservable(function () {
            return ko.utils.arrayFilter(this.tasks(), function (task) { return !task.completed(); });
        }, this);

    }

    var vm = new taskListViewModel();
    ko.applyBindings(vm);
    // Start the connection
    $.connection.hub.start(function () { vm.init(); });

});

Coollabr.Tasks = function () {

    var toggleTask = function (sender) {
        var element = document.getElementById(sender.id);

        if ($(element).find('.task-edit-wrap').css('display') == 'block') {
            $(element).find('.task-edit i').removeClass('icon-arrow-up');
            $(element).find('.task-edit i').addClass('icon-arrow-down');
        }
        else {
            $(element).find('.task-edit i').removeClass('icon-arrow-down');
            $(element).find('.task-edit i').addClass('icon-arrow-up');
        }

        $(element).find('.task-edit-wrap').toggle();
    };

    return {
        ToggleTask: toggleTask
    };

} ();


