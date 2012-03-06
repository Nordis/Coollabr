using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoolabrThird.Models;
using SignalR.Hubs;
using Coollabr.Utilities.ExtensionMethods;

namespace CoolabrThird.Hubs
{
    public class Tasks : Hub
    {
        public bool Test(string message)
        {
            Caller.test(message);
            return true;
        }

        public void Send(string message)
        {
            //Call the addMessage method on all clients.
            Clients.addMessage(message);
            Caller.updateTask("Message sent");
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        public bool Add(Task newTask)
        {
            try
            {
                if (newTask.Title.Length < 5)
                    throw new Exception("Task title too short");

                using (var session = MvcApplication.Store.OpenSession())
                {
                    var user = session.GetCurrentUser();
                    var task = new Task();
                    task.Title = newTask.Title;
                    task.Completed = newTask.Completed;
                    task.LastUpdated = DateTime.Now;
                    task.UserId = user.Id;
                    task.UserFullName = user.FullName;
                    session.Store(task);
                    session.SaveChanges();
                    Clients.taskAdded(task);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Caller.reportError(ex.Message);
                return false;
            }

        }

        public void GetAllTasks()
        {
            using (var session = MvcApplication.Store.OpenSession())
            {
                var tasks = session.Query<Task>().OrderByDescending(t => t.LastUpdated).ToArray();
                Caller.taskAll(tasks.OrderBy(t => t.Completed));
            }
        }

        /// <summary>
        /// Update a task using
        /// </summary>
        public bool Update(Task updatedTask)
        {
            using (var session = MvcApplication.Store.OpenSession())
            {
                var oldTask = session.Query<Task>().FirstOrDefault(t => t.Id == updatedTask.Id);
                try
                {
                    if (updatedTask.Title.Length < 5)
                        throw new Exception("Title length to short");

                    if (oldTask == null)
                        return false;
                    else
                    {
                        oldTask.Title = updatedTask.Title;
                        oldTask.Completed = updatedTask.Completed;
                        oldTask.LastUpdated = DateTime.Now;
                        session.SaveChanges();
                        Clients.taskUpdated(oldTask);
                        Caller.reportSuccess("Task updated!");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Caller.reportError("Content too short");
                    return false;
                }
            }

        }

        /// <summary>
        /// Delete the task
        /// </summary>
        public bool Remove(string id)
        {
            try
            {
                using (var session = MvcApplication.Store.OpenSession())
                {
                    var task = session.Query<Task>().FirstOrDefault(t => t.Id == id);
                    session.Delete(task);
                    session.SaveChanges();
                    Clients.taskRemoved(id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Caller.reportError("Error");
                return false;
            }
        }
    }
}