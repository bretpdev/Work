# (**RETIRED**) Skip-Comments-from-OneLINK-to-COMPASS-Cleanup
Skip Comments from OneLINK to COMPASS Cleanup

Recently a problem was identified with UTLWK07, the script that moves skip comments (those the start with a 'K') from OneLINK to COMPASS.  Notes entered into OneLINK on Friday were not being moved over.

This SAS report will identify all the affected comments, retrieve the comment along with the effective date so that a new script can process the file and enter the necessary "make-up" comment in COMPASS.

The SAS report needs to identify ALL comments entered on Friday from when UTLWK07 was promoted up until the time we changed the scheduling to include a Saturday run.
