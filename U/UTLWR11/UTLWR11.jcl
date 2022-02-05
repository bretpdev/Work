#UTLWR11.jcl PLUSGB Endorsement Field Setting Cleanup
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWR11.LWR11R1
then
rm ${reportdir}/ULWR11.LWR11R1
fi
if test -a ${reportdir}/ULWR11.LWR11R2
then
rm ${reportdir}/ULWR11.LWR11R2
fi
if test -a ${reportdir}/ULWR11.LWR11RZ
then
rm ${reportdir}/ULWR11.LWR11RZ
fi

# run the program

sas ${codedir}/UTLWR11.sas -log ${reportdir}/ULWR11.LWR11R1  -mautosource
