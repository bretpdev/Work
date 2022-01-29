#UTLWO40.jcl 120 Day Payment Changes Needed
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO40.LWO40R1
then
rm ${reportdir}/ULWO40.LWO40R1
fi
if test -a ${reportdir}/ULWO40.LWO40R2
then
rm ${reportdir}/ULWO40.LWO40R2
fi
if test -a ${reportdir}/ULWO40.LWO40R3
then
rm ${reportdir}/ULWO40.LWO40R3
fi
if test -a ${reportdir}/ULWO40.LWO40R4
then
rm ${reportdir}/ULWO40.LWO40R4
fi
if test -a ${reportdir}/ULWO40.LWO40RZ
then
rm ${reportdir}/ULWO40.LWO40RZ
fi

# run the program

sas ${codedir}/UTLWO40.sas -log ${reportdir}/ULWO40.LWO40R1  -mautosource
