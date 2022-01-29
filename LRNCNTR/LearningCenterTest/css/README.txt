The css directory is a container for the theme-named directories that will be
available to the application. The actual css files are placed in the theme-named
directories under this directory, because each theme applies styles differently.

The code in ElSea.jsp that loads a style sheet chooses the theme name dynamically,
but the rest of the path to the css file is hard-coded based on the type of page
being viewed, which is reflected by the navigation button selected.

For example, clicking on the "Procedures" navigation button takes the user to the
"procedures" page type, and applies the style sheet /css/{theme}/procedures.css.

Therefore, each theme directory is required to have a style sheet named after
each page type, plus a "general.css" that defines the site's general visual layout. 