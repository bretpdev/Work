#UTLWG5H.jcl Approval and Pending Accounts not in Staff Review
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG5H.LWG5HR1
then
rm ${reportdir}/ULWG5H.LWG5HR1
fi
if test -a ${reportdir}/ULWG5H.LWG5HR2
then
rm ${reportdir}/ULWG5H.LWG5HR2
fi
if test -a ${reportdir}/ULWG5H.LWG5HR3
then
rm ${reportdir}/ULWG5H.LWG5HR3
fi

# run the program

sas ${codedir}/UTLWG5H.sas -log ${reportdir}/ULWG5H.LWG5HR1  -mautosource
