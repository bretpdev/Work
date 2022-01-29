#UTLWBK9.jcl TILP CHAPTER 13 BANKRUPTCY REVIEW
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWBK9.LWBK9R2
then
rm ${reportdir}/ULWBK9.LWBK9R2
fi
if test -a ${reportdir}/ULWBK9.LWBK9RZ
then
rm ${reportdir}/ULWBK9.LWBK9RZ
fi

# run the program

sas ${codedir}/UTLWBK9.sas -log ${reportdir}/ULWBK9.LWBK9R1  -mautosource
