using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.ToastMessage
{
    public static class MyToastMessages
    {
        // Predefined success messages
        public static ToastMessage RecordSaved => new ToastMessage
        {
            Title = "Success",
            Content = "Record saved successfully.",
            CssClass = "e-toast-success"
        };

        public static ToastMessage RecordUpdated => new ToastMessage
        {
            Title = "Success",
            Content = "Record updated successfully.",
            CssClass = "e-toast-success"
        };

        public static ToastMessage RecordDeleted => new ToastMessage
        {
            Title = "Success",
            Content = "Record deleted successfully.",
            CssClass = "e-toast-success"
        };

        // Predefined error messages
        public static ToastMessage ErrorLoading => new ToastMessage
        {
            Title = "Error",
            Content = "Failed to load data.",
            CssClass = "e-toast-danger"
        };

        public static ToastMessage ErrorSaving => new ToastMessage
        {
            Title = "Error",
            Content = "Failed to save record.",
            CssClass = "e-toast-danger"
        };

        public static ToastMessage NetworkError => new ToastMessage
        {
            Title = "Network Error",
            Content = "Unable to connect to server.",
            CssClass = "e-toast-danger"
        };

        // Predefined warning messages
        public static ToastMessage ValidationError => new ToastMessage
        {
            Title = "Validation Error",
            Content = "Please check your input and try again.",
            CssClass = "e-toast-warning"
        };

        public static ToastMessage UnsavedChanges => new ToastMessage
        {
            Title = "Warning",
            Content = "You have unsaved changes.",
            CssClass = "e-toast-warning"
        };

        // Dynamic message creators
        public static ToastMessage CreateError(string content) => new ToastMessage
        {
            Title = "Error",
            Content = content,
            CssClass = "e-toast-danger"
        };

        public static ToastMessage CreateSuccess(string content) => new ToastMessage
        {
            Title = "Success",
            Content = content,
            CssClass = "e-toast-success"
        };

        public static ToastMessage CreateWarning(string content) => new ToastMessage
        {
            Title = "Warning",
            Content = content,
            CssClass = "e-toast-warning"
        };

        public static ToastMessage CreateInfo(string content) => new ToastMessage
        {
            Title = "Information",
            Content = content,
            CssClass = "e-toast-info"
        };

        // Custom message creator
        public static ToastMessage Create(string title, string content, string cssClass, int timeout = 5000) =>
            new ToastMessage
            {
                Title = title,
                Content = content,
                CssClass = cssClass,
                Timeout = timeout
            };
    }
}
